using BeUP.ViewModels;

namespace BeUP.View;

public partial class ChosenBreakfastPage : ContentPage
{
	public ChosenBreakfastPage(ChosenBreakfastViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

        viewModel.ChangeFavImage += ChangeFavImage;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var viewModel = BindingContext as ChosenBreakfastViewModel;

        viewModel.OnAppearing();
    }

    public void ChangeFavImage()
    {
        var viewModel = BindingContext as ChosenBreakfastViewModel;

        FavImage.Source = viewModel.FavImage;
    }

    private void EditorHour_TextChanged(object sender, TextChangedEventArgs e)
    {
        var viewModel = BindingContext as ChosenBreakfastViewModel;

        viewModel.Hours = EditorHours.Text;
    }

    private void EditorMinute_TextChanged(object sender, TextChangedEventArgs e)
    {
        var viewModel = BindingContext as ChosenBreakfastViewModel;

        viewModel.Minutes = EditorMinutes.Text;
    }
}