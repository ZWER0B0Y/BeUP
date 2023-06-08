using BeUP.Models;
using BeUP.Services;
using BeUP.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace BeUP.ViewModels;

public partial class BreakfastsViewModel : BaseViewModel
{
    public ObservableCollection<Breakfast> Breakfasts { get; set; }
    public ObservableCollection<Breakfast> TempBreakfasts { get; set; }

    [ObservableProperty]
    bool isRefreshing;

    public BreakfastsViewModel() 
    {
        Breakfasts = new ObservableCollection<Breakfast>();
        TempBreakfasts = new ObservableCollection<Breakfast>();
    }

    public async void OnAppearing()
    {
        await GetBreakfastsAsync();
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
                    { "Breakfast", breakfast }
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

            for (int i = 0; i < Breakfasts.Count(); i++)
            {
                if (breakfast.Id == Breakfasts[i].Id) 
                {
                    Breakfasts[i] = breakfast;
                    break;
                }
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
    public async Task GetBreakfastsAsync() 
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            var breakfasts = await BreakfastService.GetBreakfasts();

            if (Breakfasts.Count == 0)
            {
                foreach (var breakfast in breakfasts)
                {
                    Breakfasts.Add(breakfast);
                }
                return;
            }


            TempBreakfasts.Clear();
            foreach (var breakfast in breakfasts)
            {
                TempBreakfasts.Add(breakfast);
            }

            for (int i = 0; i < TempBreakfasts.Count(); i++)
            {
                for (int j = 0; j < Breakfasts.Count(); j++)
                {
                    if (Breakfasts[j] != TempBreakfasts[i] && Breakfasts[j].Id == TempBreakfasts[i].Id)
                    {
                        Breakfasts[j] = TempBreakfasts[i];
                    }
                }

                if (Breakfasts.Contains(TempBreakfasts[i]) != true)
                {
                    Breakfasts.Add(TempBreakfasts[i]);
                }
            }

            for (int j = 0; j < Breakfasts.Count(); j++)
            {
                if (TempBreakfasts.Contains(Breakfasts[j]) != true)
                {
                    Breakfasts.RemoveAt(j);
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

    [RelayCommand]
    public async Task RefreshBreakfastsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            var breakfasts = await BreakfastService.GetBreakfasts();

            if (Breakfasts.Count != 0)
                Breakfasts.Clear();

            foreach (var breakfast in breakfasts)
            {
                Breakfasts.Add(breakfast);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Помилка!", $"Неможливо отримати рецепти: \n{ex.Message}", "OK");
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    public async Task ShuffleBreakfastsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            Random rng = new Random();
            int n = Breakfasts.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Breakfast value = Breakfasts[k];
                Breakfasts[k] = Breakfasts[n];
                Breakfasts[n] = value;
                await Task.Delay(10);
            }

            await Task.Delay(10);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Помилка!", $"Неможливо перемішати рецепти: \n{ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}