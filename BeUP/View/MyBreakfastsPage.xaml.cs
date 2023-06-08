using BeUP.Services;
using BeUP.ViewModels;
using System.Reflection;

namespace BeUP.View;

public partial class MyBreakfastsPage : ContentPage
{
    public MyBreakfastsPage(MyBreakfastsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var viewModel = BindingContext as MyBreakfastsViewModel;

        viewModel.OnAppearing();
    }
}