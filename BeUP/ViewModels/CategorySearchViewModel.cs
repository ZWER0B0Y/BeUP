using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BeUP.Models;
using BeUP.Services;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using BeUP.View;

namespace BeUP.ViewModels;

[QueryProperty("Category", "Category")]
public partial class CategorySearchViewModel : BaseViewModel
{
    public ObservableCollection<Breakfast> SearchedBreakfasts { get; set; }

    [ObservableProperty]
    string category;

    public CategorySearchViewModel()
    {
        SearchedBreakfasts = new ObservableCollection<Breakfast>();
    }

    public async void OnAppearing()
    {
        await GetSearchedBreakfastsAsync();
    }

    [RelayCommand]
    async Task GoToDetailsAsync(Breakfast breakfast)
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

            await Shell.Current.GoToAsync($"{nameof(BreakfastDetailsPage)}", true,
                new Dictionary<string, object>
                {
                    {"Breakfast", breakfast }
                });
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Помилка!", $"Неможливо отримати деталі: \n{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
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

            var id1 = breakfast.Id;

            var id2 = SearchedBreakfasts[breakfast.Id - 1];

            SearchedBreakfasts[breakfast.Id - 1] = breakfast;

            await BreakfastService.SaveChanges(breakfast);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Помилка!", $"Неможливо редагувати улюблене: \n{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    async Task GetSearchedBreakfastsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var Breakfasts = await BreakfastService.GetBreakfasts();

            if (SearchedBreakfasts.Count != 0)
                SearchedBreakfasts.Clear();

            foreach (var breakfast in Breakfasts)
            {
                if (breakfast.CategoryList.Contains(Category) == true)
                {
                    SearchedBreakfasts.Add(breakfast);
                }
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Помилка!", $"Неможливо отримати відфільтровані рецепти: \n{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
