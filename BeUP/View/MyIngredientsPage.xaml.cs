using BeUP.ViewModels;

namespace BeUP.View;

public partial class MyIngredientsPage : ContentPage
{
	public MyIngredientsPage(MyIngredientsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var viewModel = BindingContext as MyIngredientsViewModel;

        viewModel.OnAppearing();
    }
}