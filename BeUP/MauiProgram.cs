using BeUP.Services;
using BeUP.View;
using BeUP.ViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;

namespace BeUP;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseLocalNotification()
            .UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Services.AddSingleton<BreakfastsViewModel>();
        builder.Services.AddSingleton<FavoritesViewModel>();
        builder.Services.AddSingleton<MyBreakfastsViewModel>();
        builder.Services.AddSingleton<CategorySearchViewModel>();
        builder.Services.AddSingleton<CategoriesViewModel>();
        builder.Services.AddSingleton<IngredientsSearchViewModel>();
        builder.Services.AddTransient<ChosenBreakfastViewModel>();
        builder.Services.AddTransient<MyBreakfastCreateViewModel>();
        builder.Services.AddTransient<IngredientsListViewModel>();
        builder.Services.AddTransient<BreakfastDetailsViewModel>();
        builder.Services.AddTransient<MyBreakfastCreateViewModel>();
        builder.Services.AddTransient<MyCategoriesViewModel>();
        builder.Services.AddTransient<MyIngredientsViewModel>();
        builder.Services.AddTransient<MyBreakfastEditViewModel>();

        builder.Services.AddSingleton<BreakfastsPage>();
        builder.Services.AddSingleton<FavoritesPage>();
        builder.Services.AddSingleton<MyBreakfastsPage>();
        builder.Services.AddSingleton<CategorySearchPage>();
        builder.Services.AddSingleton<CategoriesPage>();
        builder.Services.AddSingleton<IngredientsSearchPage>();
        builder.Services.AddTransient<ChosenBreakfastPage>();
        builder.Services.AddTransient<MyBreakfastCreatePage>();
        builder.Services.AddTransient<IngredientsListPage>();
        builder.Services.AddTransient<BreakfastDetailsPage>();
        builder.Services.AddTransient<MyBreakfastCreatePage>();
        builder.Services.AddTransient<MyCategoriesPage>();
        builder.Services.AddTransient<MyIngredientsPage>();
        builder.Services.AddTransient<MyBreakfastEditPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
