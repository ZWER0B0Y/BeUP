using BeUP.View;
using BeUP.Services;
using BeUP.Models;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeUP.ViewModels;

[QueryProperty("IngredientsList", "IngredientsList")]
public partial class IngredientsSearchViewModel : BaseViewModel
{
    [ObservableProperty]
    public List<string> ingredientsList;

    public ObservableCollection<string> SelectedIngredients { get; set; }
    public ObservableCollection<Breakfast> SearchedBreakfasts { get; set; }

    public IngredientsSearchViewModel()
    {
        SelectedIngredients = new ObservableCollection<string>();
        SearchedBreakfasts = new ObservableCollection<Breakfast>();
    }

    public async void OnAppearing()
    {
        await GetIngredientsAsync();
        await GetBreakfastsAsync();
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
                { "Breakfast", breakfast }
            });
    }

    [RelayCommand]
    async Task GetIngredientsAsync()
    {
        if (IsBusy)
            return;

        if (IngredientsList == null)
            return;

        try
        {
            IsBusy = true;

            if (SelectedIngredients.Count != 0)
                SelectedIngredients.Clear();

            foreach (var category in IngredientsList)
            {
                SelectedIngredients.Add(category);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Помилка!", $"Неможливо отримати інгредієнти: \n{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

        [RelayCommand]
    async Task GetBreakfastsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            var breakfasts = await BreakfastService.GetBreakfasts();

            if (SearchedBreakfasts.Count != 0)
                SearchedBreakfasts.Clear();

            foreach (var breakfast in breakfasts)
            {
                var source = SelectedIngredients;
                var compare = breakfast.IngredientsList;
                var result = source.Intersect(compare);

                if (result.Count() == SelectedIngredients.Count()) 
                {
                    SearchedBreakfasts.Add(breakfast);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Помилка!", $"Неможливо отримати категорії: \n{ex.Message}", "OK");
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

            if (breakfast.Id - 1 != SearchedBreakfasts.Count)
            {
                SearchedBreakfasts[breakfast.Id - 1] = breakfast;
            }
            else
            {
                SearchedBreakfasts[breakfast.Id - 2] = breakfast;
            }

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
    async Task AddIngredientsAsync()
    {
        try
        {
            IsBusy = true;

            List<string> selectedIngredients = new List<string>();

            foreach (var ingredient in SelectedIngredients) 
            {
                selectedIngredients.Add(ingredient);
            }

            await Shell.Current.GoToAsync($"{nameof(IngredientsListPage)}", true,
                new Dictionary<string, object>
                {
                    { "IngredientsList", selectedIngredients }
                });
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Помилка!", $"Неможливо додати інгредієнти: \n{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    async Task RemoveIngredientsAsync(string ingredient)
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            SelectedIngredients?.Remove(ingredient);
            IngredientsList.Remove(ingredient);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Помилка!", $"Неможливо видалити інгредієнти: \n{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            await Task.Run(GetBreakfastsAsync);
        }
    }
}
