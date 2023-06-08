using BeUP.ViewModels;

namespace BeUP.View;

public partial class CategorySearchPage : ContentPage
{
	public CategorySearchPage(CategorySearchViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var viewModel = BindingContext as CategorySearchViewModel;

        viewModel.OnAppearing();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}