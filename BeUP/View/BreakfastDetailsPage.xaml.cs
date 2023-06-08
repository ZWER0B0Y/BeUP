using BeUP.ViewModels;

namespace BeUP.View;

public partial class BreakfastDetailsPage : ContentPage
{
	public BreakfastDetailsPage(BreakfastDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

        viewModel.DeletePage += RemovePage;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    public void RemovePage( Page page) 
    {
        Navigation.RemovePage(page);
    }
}