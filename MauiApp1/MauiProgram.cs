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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddHttpClient<IProductService, ProductService>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            builder.Services.AddSingleton<IProductService, ProductService>();

            builder.Services.AddTransient<ProductListViewModel>();
            builder.Services.AddTransient<ProductDetailViewModel>();

            builder.Services.AddTransient<ProductListPage>();
            builder.Services.AddTransient<ProductDetailPage>();

#if DEBUG
            builder.Logging.AddDebug();
            builder.Logging.SetMinimumLevel(LogLevel.Information);
#endif

            return builder.Build();
        }
    }
}