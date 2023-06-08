using BeUP.Services;
using BeUP.ViewModels;
using System.Reflection;

namespace BeUP.View;

public partial class MyBreakfastCreatePage : ContentPage
{
    public MyBreakfastCreatePage(MyBreakfastCreateViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var viewModel = BindingContext as MyBreakfastCreateViewModel;

        viewModel.OnAppearing();
    }

    private void EditorName_TextChanged(object sender, TextChangedEventArgs e)
    {
        var viewModel = BindingContext as MyBreakfastCreateViewModel;

        viewModel.Name = EditorName.Text;
    }

    private void EditorDescription_TextChanged(object sender, TextChangedEventArgs e)
    {
        var viewModel = BindingContext as MyBreakfastCreateViewModel;

        viewModel.Description = EditorDescription.Text;
    }

    private void EditorRecipe_TextChanged(object sender, TextChangedEventArgs e)
    {
        var viewModel = BindingContext as MyBreakfastCreateViewModel;

        viewModel.Recipe = EditorRecipe.Text;
    }
}