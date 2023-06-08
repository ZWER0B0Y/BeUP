using BeUP.Models;
using BeUP.Services;
using BeUP.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BeUP.ViewModels;

public partial class CategoriesViewModel : BaseViewModel
{
    public ObservableCollection<string> Categories { get; set; }

    public CategoriesViewModel()
    {
        Categories = new ObservableCollection<string>();
    }

    public async void OnAppearing()
    {
        await GetCategoriesAsync();
    }

    [RelayCommand]
    async Task GoToCategoryAsync(string category)
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            if (category is null)
            {
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(CategorySearchPage)}?Category={category}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Помилка!", $"Неможливо перейти до категорії: \n{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task GetCategoriesAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            var breakfasts = await BreakfastService.GetBreakfasts();
            List<string> categoryList = new List<string>();

            if (Categories.Count != 0)
                Categories.Clear();

            foreach (var breakfast in breakfasts)
            {
                for (int i = 0; i < breakfast.CategoryList.Count(); i++) 
                {
                    if (categoryList.Contains(breakfast.CategoryList[i]) == false) 
                    {
                        categoryList.Add(breakfast.CategoryList[i]);
                    }
                }
            }

            categoryList.Sort();

            foreach (var category in categoryList)
            {   
                Categories.Add(category);
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
}
