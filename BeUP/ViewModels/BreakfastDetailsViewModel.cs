using BeUP.Models;
using BeUP.ViewModels;
using BeUP.Services;
using BeUP.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Diagnostics;
using BeUP.View;

namespace BeUP.ViewModels;

[QueryProperty("Breakfast", "Breakfast")]
public partial class BreakfastDetailsViewModel : BaseViewModel
{
    [ObservableProperty]
    Breakfast breakfast;

    public event Action<Page> DeletePage;

    public BreakfastDetailsViewModel()
    {

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

            RefreshAsync(breakfast);

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
    async Task ChooseAsync(Breakfast breakfast)
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

            Collection<Breakfast> Breakfasts = new Collection<Breakfast>();
            var breakfasts = await BreakfastService.GetBreakfasts();

            foreach (var cBreakfast in breakfasts)
            {
                if (cBreakfast.Chosen == 1)
                {
                    cBreakfast.Chosen = 0;
                }
                if (cBreakfast.Id == breakfast.Id)
                {
                    cBreakfast.Chosen = 1;
                    breakfast.Chosen = 1;
                }
                Breakfasts.Add(cBreakfast);
            }

            await BreakfastService.UpdateAllData(Breakfasts);

            RefreshAsync(breakfast);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Помилка!", $"Неможливо обрати рецепт: \n{ex.Message}", "OK");
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

        await Shell.Current.GoToAsync($"{nameof(BreakfastDetailsPage)}", false,
            new Dictionary<string, object>
            {
                { "Breakfast", breakfast }
            });

        DeletePage?.Invoke(page);
    }
}