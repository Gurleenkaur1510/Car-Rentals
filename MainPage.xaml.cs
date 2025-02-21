using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.Logging; // Provides logging utilities

namespace CarRentalSystem
{
    // Represents the main page of the application
    public partial class MainPage : ContentPage
    {
        // Constructor for the MainPage class
        public MainPage()
        {
            InitializeComponent(); // Initializes the components defined in the associated XAML file

            // Event handler for when the Blazor WebView is initialized
            blazorWebView.BlazorWebViewInitialized += (sender, args) =>
            {
                Console.WriteLine("Blazor WebView Initialized."); // Logs a message to the console
            };
        }
    }
}
