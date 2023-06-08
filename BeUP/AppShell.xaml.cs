using BeUP.View;
using BeUP.ViewModels;

namespace BeUP;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(BreakfastsPage), typeof(BreakfastsPage));
        Routing.RegisterRoute(nameof(BreakfastDetailsPage), typeof(BreakfastDetailsPage));
        Routing.RegisterRoute(nameof(CategoriesPage), typeof(CategoriesPage));
        Routing.RegisterRoute(nameof(CategorySearchPage), typeof(CategorySearchPage));
        Routing.RegisterRoute(nameof(IngredientsListPage), typeof(IngredientsListPage));
        Routing.RegisterRoute(nameof(IngredientsSearchPage), typeof(IngredientsSearchPage));
        Routing.RegisterRoute(nameof(MyBreakfastCreatePage), typeof(MyBreakfastCreatePage));
        Routing.RegisterRoute(nameof(MyCategoriesPage), typeof(MyCategoriesPage));
        Routing.RegisterRoute(nameof(MyIngredientsPage), typeof(MyIngredientsPage));
        Routing.RegisterRoute(nameof(MyBreakfastEditPage), typeof(MyBreakfastEditPage));
        Routing.RegisterRoute(nameof(ChosenBreakfastPage), typeof(ChosenBreakfastPage));
    }

    private void ShellContent_Appearing(object sender, EventArgs e)
    {

    }
}
