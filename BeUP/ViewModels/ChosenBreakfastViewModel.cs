using CommunityToolkit.Mvvm.ComponentModel;
using BeUP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using BeUP.Services;
using BeUP.View;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using Plugin.LocalNotification;

namespace BeUP.ViewModels;

public partial class ChosenBreakfastViewModel : BaseViewModel
{
    [ObservableProperty]
    Breakfast breakfast;

    public event Action ChangeFavImage;

    public string FavImage;

    public string Hours;
    public string Minutes;

    public ChosenBreakfastViewModel()
    {

    }

    public async void OnAppearing()
    {
        await GetChosenBreakfastAsync();
    }


    [RelayCommand]
    async Task GetChosenBreakfastAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            Breakfast = null;

            Breakfast breakfast = new Breakfast();
            breakfast.Id = -1;
            breakfast.Image = "bbregg";
            breakfast.Favorite = 0;
            breakfast.Name = "Недоступно";
            breakfast.Description = "Недоступно";
            breakfast.Recipe = "Недоступно";
            breakfast.Category = "Недоступно";
            breakfast.Ingredients = "Недоступно";

            var chosenBreakfasts = await BreakfastService.GetBreakfasts();

            for (int i = 0; i < chosenBreakfasts.Count(); i++)
            {
                if (chosenBreakfasts.ElementAt(i).Chosen == 1)
                {
                    Breakfast = chosenBreakfasts.ElementAt(i);
                    break;
                }
            }

            if (Breakfast == null) 
            {
                Breakfast = breakfast;

                await Shell.Current.DisplayAlert("Увага!", "Спочатку додайте будь-який рецепт до обраного.", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Помилка!", $"Неможливо отримати обраний рецепт: \n{ex.Message}", "OK");
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

            if (breakfast.Id == -1)
            {
                await Shell.Current.DisplayAlert("Увага!", "Спочатку додайте будь-який рецепт до обраного.", "OK");
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

            ChangeFavImageCom(breakfast.Favorite);

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

    public void ChangeFavImageCom(int image)
    {
        if (image == 1)
        {
            FavImage = "BeUP/Resources/Images/favorite_active";
        }
        else
        {
            if (image == 0)
            {
                FavImage = "BeUP/Resources/Images/favorite_passive"; ;
            }
            else
            {
                Shell.Current.DisplayAlert("Помилка!", "Неможливо редагувати улюблене.", "OK");
            }
        }

        ChangeFavImage?.Invoke();
    }

    [RelayCommand]
    async Task NotificationAsync()
    {
        if (Breakfast.Id == -1)
        {
            await Shell.Current.DisplayAlert("Увага!", "Спочатку додайте будь-який рецепт до обраного.", "OK");
            return;
        }

        if ((Hours == null || Minutes == null) || (Hours == "" || Minutes == "")) 
        {
            await Shell.Current.DisplayAlert("Увага!", "Будь-ласка, введіть час для повідомлення.", "OK");
            return;
        }

        int hours = int.Parse(Hours);
        int minutes = int.Parse(Minutes);

        if ((hours < 0 || hours > 23) || (minutes < 0 || minutes > 59))
        {
            await Shell.Current.DisplayAlert("Увага!", "Будь-ласка, введіть час в правильному форматі: \n Години: 0 - 23; \n Хвилини: 0 - 59.", "OK");
            return;
        }

        await NotificationService.MakeNotification(hours, minutes);

        if (Hours.Length == 1)
            Hours = "0" + Hours;

        if (Minutes.Length == 1)
            Minutes = "0" + Minutes;

        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == true)
        {
            if (hours < DateTime.Now.Hour || minutes <= DateTime.Now.Minute)
            {
                await Shell.Current.DisplayAlert("Готово!", $"Ми нагадаємо вам про рецепт завтра у {Hours}:{Minutes}.", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Готово!", $"Ми нагадаємо вам про рецепт сьогодні у {Hours}:{Minutes}.", "OK");
            }
        }
        else 
        {
            await Shell.Current.DisplayAlert("Помилка!", "Щоб отримувати повідомлення про рецепт, будь-ласка, надайте цьому додатку дозвіл надсилати вам повідомлення.", "OK");
        }
    }
}
