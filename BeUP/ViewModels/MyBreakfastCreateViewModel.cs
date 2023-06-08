using CommunityToolkit.Mvvm.Input;
using System;
using BeUP.View;
using BeUP.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using CommunityToolkit.Maui.Storage;
using System.Threading;
using BeUP.Models;
using Plugin.LocalNotification;

namespace BeUP.ViewModels;

[QueryProperty("IngredientsList", "IngredientsList")]
[QueryProperty("CategoriesList", "CategoriesList")]
public partial class MyBreakfastCreateViewModel : BaseViewModel
{
    [ObservableProperty]
    public string imageCur;

    public string Name;
    public string Description;
    public string Recipe;

    [ObservableProperty]
    public List<string> ingredientsList;

    [ObservableProperty]
    public List<string> categoriesList;

    public ObservableCollection<string> SelectedIngredients { get; set; }
    public ObservableCollection<string> SelectedCategories { get; set; }

    public MyBreakfastCreateViewModel()
    {
        SelectedIngredients = new ObservableCollection<string>();
        SelectedCategories = new ObservableCollection<string>();
        ImageCur = "spun";
    }

    public async void OnAppearing()
    {
        await GetIngredientsAsync();
        await GetCategoriesAsync();
    }

    [RelayCommand]
    async Task  SaveRecipeAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            if (Name == null)
                throw new Exception("Напишіть назву рецепту.");

            if (Description == null)
                throw new Exception("Напишіть опис до рецепту.");

            if (Recipe == null)
                throw new Exception("Напишіть рецепт.");

            Breakfast newBreakfast = new Breakfast();
            newBreakfast.Name = Name;
            newBreakfast.Description = Description;
            newBreakfast.Recipe = Recipe;
            newBreakfast.Image = ImageCur;
            string cat;
            if (SelectedCategories.Count == 0)
                cat = "Пусто";
            else
                cat = string.Join(",", SelectedCategories);
            newBreakfast.Category = cat;
            string ing;
            if (SelectedIngredients.Count == 0)
                ing = "Пусто";
            else
                ing = string.Join(",", SelectedIngredients);
            newBreakfast.Ingredients = ing;
            newBreakfast.Chosen = 0;
            newBreakfast.Favorite = 0;
            newBreakfast.Own = 1;

            await BreakfastService.AddRecord(newBreakfast);

            await Shell.Current.GoToAsync("..", true);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Увага!", $"Неможливо створити новий рецепт: \n{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    async Task GetImageAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            var result = await FilePicker.PickAsync(
                new PickOptions
                {
                    PickerTitle = "Оберіть фотографію для рецепту.",
                    FileTypes = FilePickerFileType.Images
                });

            if (result == null)
                throw new Exception("Фотографію не обрано.");

            var stream = await result.OpenReadAsync();

            ImageCur = result.FullPath;

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Увага!", $"Неможливо отримати зображення: \n{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
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
    async Task GetCategoriesAsync()
    {
        if (IsBusy)
            return;

        if (CategoriesList == null)
            return;

        try
        {
            IsBusy = true;

            if (SelectedCategories.Count != 0)
                SelectedCategories.Clear();

            foreach (var category in CategoriesList)
            {
                SelectedCategories.Add(category);
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
    async Task GoToIngredientsAsync()
    {
        try
        {
            IsBusy = true;

            List<string> selectedIngredients = new List<string>();

            foreach (var ingredient in SelectedIngredients)
            {
                selectedIngredients.Add(ingredient);
            }

            await Shell.Current.GoToAsync($"{nameof(MyIngredientsPage)}", true,
                new Dictionary<string, object>
                {
                    { "IngredientsList", selectedIngredients }
                });
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Помилка!", $"Неможливо піти до інгредієнтів: \n{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    async Task GoToCategoriesAsync()
    {
        try
        {
            IsBusy = true;

            List<string> selectedCategories = new List<string>();

            foreach (var category in SelectedCategories)
            {
                selectedCategories.Add(category);
            }

            await Shell.Current.GoToAsync($"{nameof(MyCategoriesPage)}", true,
                new Dictionary<string, object>
                {
                    { "CategoriesList", selectedCategories }
                });
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Помилка!", $"Неможливо піти до категорій: \n{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
