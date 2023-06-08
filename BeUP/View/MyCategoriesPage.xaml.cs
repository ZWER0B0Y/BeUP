using BeUP.ViewModels;

namespace BeUP.View;

public partial class MyCategoriesPage : ContentPage
{
    public MyCategoriesPage(MyCategoriesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var viewModel = BindingContext as MyCategoriesViewModel;

        viewModel.OnAppearing();
    }
}