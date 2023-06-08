using BeUP.Models;
using BeUP.Services;
using BeUP.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BeUP.ViewModels;

public partial class MyBreakfastsViewModel : BaseViewModel
{
    public ObservableCollection<Breakfast> MyBreakfasts { get; set; }

    public MyBreakfastsViewModel()
    {
        MyBreakfasts = new ObservableCollection<Breakfast>();
    }

    public async void OnAppearing()
    {
        await GetMyBreakfastsAsync();
    }

    [RelayCommand]
    async Task GoToDetailsAsync(Breakfast breakfast)
    {
        if (breakfast is null)
        {
            return;
        }

        await Shell.Current.GoToAsync($"{nameof(BreakfastDetailsPage)}", true,
            new Dictionary<string, object>
            {
                {"Breakfast", breakfast }
            });
    }

    [RelayCommand]
    async Task CreateRecipeAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(MyBreakfastCreatePage)}", true);
    }

    [RelayCommand]
    async Task MarkFavoriteAsync(Breakfast breakfast)
    {
        if (breakfast is null)
        {
            return;
        }

        if (breakfast.Favorite == 0)
        {
            breakfast.Favorite = 1;
        }
        else
        {
            breakfast.Favorite = 0;
        }

        await BreakfastService.SaveChanges(breakfast);
        await Task.Run(GetMyBreakfastsAsync);
    }

    [RelayCommand]
    async Task EditMyBreakfastAsync(Breakfast breakfast)
    {
        if (breakfast is null)
            return;

        await Shell.Current.GoToAsync($"{nameof(MyBreakfastEditPage)}", true,
            new Dictionary<string, object>
            {
                {"Breakfast", breakfast }
            });
    }

    [RelayCommand]
    async Task DeleteMyBreakfastAsync(Breakfast breakfast) 
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            var actionSheet = await Shell.Current.DisplayAlert("Увага", $"Ви хочете видалити {breakfast.Name}?", "Так", "Ні");

            switch (actionSheet)
            {
                case true:
                    await BreakfastService.DeleteBreakfast(breakfast);
                    break;

                case false:
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Помилка!", $"Неможливо видалити {breakfast.Name}: \n{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            await Task.Run(GetMyBreakfastsAsync);
        }

    }

    [RelayCommand]
    async Task GetMyBreakfastsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var breakfasts = await BreakfastService.GetBreakfasts();

            if (MyBreakfasts.Count != 0)
                MyBreakfasts.Clear();

            foreach (var breakfast in breakfasts)
            {
                if (breakfast.Own == 1)
                {
                    MyBreakfasts.Add(breakfast);
                }
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Помилка!", $"Неможливо отримати рецепти: \n{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}

