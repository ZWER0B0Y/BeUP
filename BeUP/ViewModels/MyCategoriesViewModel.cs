using CommunityToolkit.Mvvm.ComponentModel;
using System;
using BeUP.Models;
using BeUP.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace BeUP.ViewModels;

[QueryProperty("CategoriesList", "CategoriesList")]
public partial class MyCategoriesViewModel : BaseViewModel
{
    [ObservableProperty]
    public List<string> categoriesList;
    public ObservableCollection<StringBoolCheck> AllCategories { get; set; }
    public List<string> SelectedCategories { get; set; }

    public MyCategoriesViewModel()
    {
        CategoriesList = new List<string>();
        AllCategories = new ObservableCollection<StringBoolCheck>();
        SelectedCategories = new List<string>();
    }

    public async void OnAppearing()
    {
        await GetCategoriesAsync();
    }

    [RelayCommand]
    async Task GetCategoriesAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var breakfasts = await BreakfastService.GetBreakfasts();
            List<string> categoriesList = new List<string>();

            if (AllCategories.Count() != 0)
                AllCategories.Clear();

            foreach (var breakfast in breakfasts)
            {
                for (int i = 0; i < breakfast.CategoryList.Count(); i++)
                {
                    if (categoriesList.Contains(breakfast.CategoryList[i]) == false)
                    {
                        categoriesList.Add(breakfast.CategoryList[i]);
                    }
                }
            }

            categoriesList.Sort();

            foreach (string category in categoriesList)
            {
                StringBoolCheck temp = new StringBoolCheck();
                temp.Name = category;

                if (CategoriesList.Contains(category) == true)
                {
                    temp.Chosen = true;
                    SelectedCategories.Add(category);
                }
                else
                {
                    temp.Chosen = false;
                }

                AllCategories.Add(temp);
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
    async Task DoAsync(StringBoolCheck Category)
    {
        for (int i = 0; i < AllCategories.Count(); i++)
        {
            var category = AllCategories[i];
            if (AllCategories[i].Name == Category.Name)
            {
                if (AllCategories[i].Chosen != true)
                {
                    category.Chosen = true;
                    AllCategories[i] = Category;
                    SelectedCategories.Add(category.Name);
                }
                else
                {
                    category.Chosen = false;
                    AllCategories[i] = Category;
                    SelectedCategories.Remove(category.Name);
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
                    { "CategoriesList", SelectedCategories }
                });
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Помилка!", $"Неможливо додати категорії: \n{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
