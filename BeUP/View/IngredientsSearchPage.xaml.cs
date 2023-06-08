using BeUP.ViewModels;

namespace BeUP.View;

public partial class IngredientsSearchPage : ContentPage
{
	public IngredientsSearchPage (IngredientsSearchViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var viewModel = BindingContext as IngredientsSearchViewModel;

        viewModel.OnAppearing();
    }
}