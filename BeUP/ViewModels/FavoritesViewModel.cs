using BeUP.Models;
using BeUP.Services;
using BeUP.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BeUP.ViewModels;

public partial class FavoritesViewModel : BaseViewModel
{
    public ObservableCollection<Breakfast> FavBreakfasts { get; set; }

    public FavoritesViewModel()
    {
        FavBreakfasts = new ObservableCollection<Breakfast>();
    }

    public async void OnAppearing()
    {
        await GetFavBreakfastsAsync();
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
    async Task MarkFavoriteAsync(Breakfast breakfast)
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

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
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Помилка!", $"Неможливо редагувати Улюблене: \n{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            await Task.Run(GetFavBreakfastsAsync);
        }
    }

    [RelayCommand]
    async Task GetFavBreakfastsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var favBreakfasts = await BreakfastService.GetBreakfasts();

            if (FavBreakfasts.Count != 0)
                FavBreakfasts.Clear();

            foreach (var breakfast in favBreakfasts)
            {
                if (breakfast.Favorite == 1) 
                {
                    FavBreakfasts.Add(breakfast);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Помилка!", $"Неможливо отримати улюблені рецепти: \n{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
