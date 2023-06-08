using BeUP.ViewModels;

namespace BeUP.View;

public partial class CategoriesPage : ContentPage
{
	public CategoriesPage(CategoriesViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var viewModel = BindingContext as CategoriesViewModel;

        viewModel.OnAppearing();
    }
}