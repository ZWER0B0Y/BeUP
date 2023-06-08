using BeUP.Models;
using BeUP.Services;
using BeUP.ViewModels;
using System.Reflection;

namespace BeUP.View;

public partial class FavoritesPage : ContentPage
{
    public FavoritesPage(FavoritesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var viewModel = BindingContext as FavoritesViewModel;

        viewModel.OnAppearing();
    }
}