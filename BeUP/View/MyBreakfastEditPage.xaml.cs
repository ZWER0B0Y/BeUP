using BeUP.ViewModels;

namespace BeUP.View;

public partial class MyBreakfastEditPage : ContentPage
{
	public MyBreakfastEditPage(MyBreakfastEditViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

        viewModel.DeletePage += RemovePage;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var viewModel = BindingContext as MyBreakfastEditViewModel;

        viewModel.OnAppearing();
    }

    public void RemovePage(Page page)
    {
        var viewModel = BindingContext as MyBreakfastEditViewModel;

        Navigation.RemovePage(page);
    }

    private void EditorName_TextChanged(object sender, TextChangedEventArgs e)
    {
        var viewModel = BindingContext as MyBreakfastEditViewModel;

        viewModel.Breakfast.Name = EditorName.Text;
    }

    private void EditorDescription_TextChanged(object sender, TextChangedEventArgs e)
    {
        var viewModel = BindingContext as MyBreakfastEditViewModel;

        viewModel.Breakfast.Description = EditorDescription.Text;
    }

    private void EditorRecipe_TextChanged(object sender, TextChangedEventArgs e)
    {
        var viewModel = BindingContext as MyBreakfastEditViewModel;

        viewModel.Breakfast.Recipe = EditorRecipe.Text;
    }
}