using CommunityToolkit.Mvvm.ComponentModel;
using BeUP.Models;
using BeUP.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using BeUP.Services;

namespace BeUP.ViewModels;

[QueryProperty("Breakfast", "Breakfast")]
[QueryProperty("IngredientsList", "IngredientsList")]
[QueryProperty("CategoriesList", "CategoriesList")]
public partial class MyBreakfastEditViewModel : BaseViewModel
{
    [ObservableProperty]
    Breakfast breakfast;

    [ObservableProperty]
    public List<string> ingredientsList;

    [ObservableProperty]
    public List<string> categoriesList;

    private int Check = 0;

    public ObservableCollection<string> SelectedIngredients { get; set; }
    public ObservableCollection<string> SelectedCategories { get; set; }

    public event Action<Page> DeletePage;

    public MyBreakfastEditViewModel()
    {
        SelectedIngredients = new ObservableCollection<string>();
        SelectedCategories = new ObservableCollection<string>();
    }

    public async void OnAppearing()
    {
        if (Check == 0)
        {
            await GetStartAsync();
            Check++;
        }

        await GetIngredientsAsync();
        await GetCategoriesAsync();
    }

    [RelayCommand]
    async Task GetImageAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Оберіть фотографію для рецепту.",
                FileTypes = FilePickerFileType.Images
            });

            if (result == null)
                throw new Exception("Фотографію не обрано.");

            var breakfast = Breakfast;
            breakfast.Id = Breakfast.Id;

            breakfast.Image = result.FullPath;

            string cat = string.Join(",", SelectedCategories);
            breakfast.Category = cat;

            string ing = string.Join(",", SelectedIngredients);
            breakfast.Ingredients = ing;

            RefreshAsync(breakfast);

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
    public async void RefreshAsync(Breakfast breakfast)
    {
        var page = Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault();

        await Shell.Current.GoToAsync($"{nameof(MyBreakfastEditPage)}", false,
            new Dictionary<string, object>
            {
                { "Breakfast", breakfast }
            });

        DeletePage?.Invoke(page);
    }

    [RelayCommand]
    async Task GetStartAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            if (CategoriesList == null)
            {
                foreach (var category in Breakfast.CategoryList)
                {
                    SelectedCategories.Add(category);
                }
            }

            if (IngredientsList == null)
            {
                foreach (var ingredient in Breakfast.IngredientsList)
                {
                    SelectedIngredients.Add(ingredient);
                }
            }
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

            foreach (var ingredient in IngredientsList)
            {
                SelectedIngredients.Add(ingredient);
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

    [RelayCommand]
    async Task SaveChangesAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            if (Breakfast.Name == null)
                throw new Exception("Напишіть назву рецепту.");

            if (Breakfast.Description == null)
                throw new Exception("Напишіть опис до рецепту.");

            if (Breakfast.Recipe == null)
                throw new Exception("Напишіть рецепт.");

            Breakfast changedBreakfast = new Breakfast();
            changedBreakfast.Id = Breakfast.Id;
            changedBreakfast.Name = Breakfast.Name;
            changedBreakfast.Description = Breakfast.Description;
            changedBreakfast.Recipe = Breakfast.Recipe;
            changedBreakfast.Image = Breakfast.Image;
            string cat = string.Join(",", SelectedCategories);
            changedBreakfast.Category = cat;
            string ing = string.Join(",", SelectedIngredients);
            changedBreakfast.Ingredients = ing;
            changedBreakfast.Chosen = 0;
            changedBreakfast.Favorite = 0;
            changedBreakfast.Own = 1;

            await BreakfastService.SaveChanges(changedBreakfast);

            await Shell.Current.GoToAsync("..", true);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Помилка!", $"Неможливо зберегти зміни рецепту: \n{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
