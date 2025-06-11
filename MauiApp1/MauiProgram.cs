using Microsoft.Extensions.Logging;
using ProductApp.Services;
using ProductApp.ViewModels;
using ProductApp.Views;

namespace ProductApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Rejestracja HttpClient
            builder.Services.AddHttpClient();

            // Rejestracja serwisów
            builder.Services.AddSingleton<IProductService, ProductService>();

            // Rejestracja ViewModels
            builder.Services.AddTransient<ProductListViewModel>();
            builder.Services.AddTransient<ProductDetailViewModel>();

            // Rejestracja Views
            builder.Services.AddTransient<ProductListPage>();
            builder.Services.AddTransient<ProductDetailPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}