using BeUP.View;
using BeUP.Services;
using BeUP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace BeUP.ViewModels;

[QueryProperty("IngredientsList", "IngredientsList")]
public partial class IngredientsListViewModel : BaseViewModel
{
    [ObservableProperty]
    public List<string> ingredientsList;
    public ObservableCollection<StringBoolCheck> AllIngredients { get; set; }
    public List<string> SelectedIngredients { get; set; }

    public IngredientsListViewModel()
    {
        AllIngredients = new ObservableCollection<StringBoolCheck>();
        SelectedIngredients = new List<string>();
    }

    public async void OnAppearing()
    {
        await GetIngredientsAsync();
    }

    [RelayCommand]
    async Task RefreshAsync()
    {
        await Task.Run(GetIngredientsAsync);
    }

    [RelayCommand]
    async Task GetIngredientsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var breakfasts = await BreakfastService.GetBreakfasts();
            List<string> ingredientsList = new List<string>();

            if (AllIngredients.Count() != 0)
                AllIngredients.Clear();

            foreach (var breakfast in breakfasts)
            {
                for (int i = 0; i < breakfast.IngredientsList.Count(); i++)
                {
                    if (ingredientsList.Contains(breakfast.IngredientsList[i]) == false)
                    {
                        ingredientsList.Add(breakfast.IngredientsList[i]);
                    }
                }
            }

            ingredientsList.Sort();

            foreach (string ingredient in ingredientsList)
            {
                StringBoolCheck temp = new StringBoolCheck();
                temp.Name = ingredient;

                if (IngredientsList.Contains(ingredient) == true)
                {
                    temp.Chosen = true;
                    SelectedIngredients.Add(ingredient);
                }
                else 
                {
                    temp.Chosen = false;
                }

                AllIngredients.Add(temp);
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
    async Task CheckAsync(StringBoolCheck Ingredient)
    {
        for (int i = 0; i < AllIngredients.Count(); i++)
        {
            var ingredient = AllIngredients[i];
            if (AllIngredients[i].Name == Ingredient.Name)
            {
                if (AllIngredients[i].Chosen != true)
                {
                    ingredient.Chosen = true;
                    AllIngredients[i] = Ingredient;
                    SelectedIngredients.Add(ingredient.Name);
                }
                else
                {
                    ingredient.Chosen = false;
                    AllIngredients[i] = Ingredient;
                    SelectedIngredients.Remove(ingredient.Name);
                }
            }

            await Task.Delay(10);
        }
    }

    [RelayCommand]
    async Task AddAsync()
    {
        try
        {
            IsBusy = true;

            await Shell.Current.GoToAsync("..", true,
                new Dictionary<string, object>
                {
                    { "IngredientsList", SelectedIngredients }
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
}
