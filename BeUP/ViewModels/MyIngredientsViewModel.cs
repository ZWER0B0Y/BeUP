using BeUP.Models;
using BeUP.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeUP.ViewModels;

[QueryProperty("IngredientsList", "IngredientsList")]
public partial class MyIngredientsViewModel : BaseViewModel
{
    [ObservableProperty]
    public List<string> ingredientsList;
    public ObservableCollection<StringBoolCheck> AllIngredients { get; set; }
    public List<string> SelectedIngredients { get; set; }

    public MyIngredientsViewModel()
    {
        IngredientsList = new List<string>();
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

            foreach (string ingredient in IngredientsList)
            {
                if (ingredientsList.Contains(ingredient) == false)
                {
                    StringBoolCheck temp = new StringBoolCheck();
                    temp.Name = ingredient;
                    temp.Chosen = true;
                    AllIngredients.Add(temp);
                    SelectedIngredients.Add(ingredient);
                }
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
    async Task CreateIngredientAsync(string Name)
    {
        if (Name == null || Name == "") 
        {
            await Shell.Current.DisplayAlert("Увага!", "Напиішть назву інгредієнту, для того, щоб додати його в список.", "OK");
            return;
        }

        Name = Name.ToLower();
        StringBoolCheck temp = new StringBoolCheck();
        temp.Name = $"{Name[0].ToString().ToUpper()}{Name.Substring(1)}";
        temp.Chosen = false;
        AllIngredients.Add(temp);

        await Shell.Current.DisplayAlert("Увага!", $"Інгредієнт {Name}, було додано у кінець списку.", "OK"); 
    }

    [RelayCommand]
    async Task DoAsync(StringBoolCheck Ingredient)
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
