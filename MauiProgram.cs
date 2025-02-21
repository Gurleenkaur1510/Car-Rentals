using CarRentalSystem.Backend.Services;
using Microsoft.Extensions.Logging;

namespace CarRentalSystem
{
    public static class MauiProgram
    {
        // Creates and configures the Maui application
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder(); // Starts building the Maui application

            builder
                .UseMauiApp<App>() // Specifies the main application class
                .ConfigureFonts(fonts =>
                {
                    // Adds custom fonts to the application
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Registers services for dependency injection
            builder.Services.AddSingleton<DataService>(); // Service for general data handling
            builder.Services.AddSingleton<CarService>(); // Service for managing car data
            builder.Services.AddSingleton<CustomerService>(); // Service for managing customer data

            builder.Services.AddMauiBlazorWebView(); // Adds Blazor WebView support

            // Registers SQLM (SQL Manager) with a custom connection string
            builder.Services.AddScoped<SQLM>(provider =>
                new SQLM("Server=localhost\\SQLEXPRESS02;Database=master;Trusted_Connection=True;"));

#if DEBUG
            // Adds developer tools and debug logging in debug mode
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build(); // Builds and returns the Maui application
        }
    }
}
