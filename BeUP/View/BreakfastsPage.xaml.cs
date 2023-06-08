using BeUP.Models;
using BeUP.Services;
using BeUP.ViewModels;
using System.Collections.ObjectModel;
using System.Reflection;

namespace BeUP.View;

public partial class BreakfastsPage : ContentPage
{
    private BreakfastsViewModel viewModel;

    public BreakfastsPage(BreakfastsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel = BindingContext as BreakfastsViewModel;

        viewModel.OnAppearing();
    }
}

