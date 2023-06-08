using BeUP.ViewModels;

namespace BeUP.View;

public partial class IngredientsListPage : ContentPage
{
    public IngredientsListPage(IngredientsListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var viewModel = BindingContext as IngredientsListViewModel;

        viewModel.OnAppearing();
    }
}