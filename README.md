# Car-Rentals
Project: Car Rental System
Overview
This project is a Car Rental System implemented using .NET MAUI and Blazor. The system includes functionality for managing cars, customers, and rentals. It integrates a SQL Server database for persistent data storage and uses dependency injection for service management.
File Descriptions
1. MainPage.xaml
•	Type: XAML (UI Definition)
•	Purpose: Defines the UI layout for the main page of the application. It includes a BlazorWebView to integrate Blazor components within the .NET MAUI application.
2. MainPage.xaml.cs
•	Type: C# (Code-Behind for XAML)
•	Purpose: Contains the logic for the MainPage. It handles events like initializing the BlazorWebView and logs when the view is initialized.
3. MauiProgram.cs
•	Type: C# (Application Entry Point)
•	Purpose: Configures the .NET MAUI application, including:
o	Registering services like DataService, CarService, and CustomerService.
o	Setting up dependency injection for SQLM (SQL Manager).
o	Adding support for Blazor WebView and debugging tools.
4. SQLM.cs
•	Type: C# (Database Interaction)
•	Purpose: Manages SQL Server interactions, including:
o	Fetching data for cars, customers, and rentals.
o	Adding new entries for cars, customers, and rentals.
o	Updating car availability.
o	Reusable helper methods for executing parameterized SQL queries.
5. AppAssemblyHelper.cs
•	Type: C# (Utility)
•	Purpose: Provides access to assembly metadata, which can be useful for loading resources or dynamic type discovery.
6. AdminHome.razor
•	Type: Blazor Component
•	Purpose: Represents the Admin Home page, allowing navigation to CarInventory and Add Customer pages.
7. CarInventory.razor
•	Type: Blazor Component
•	Purpose:
o	Displays a list of cars available in the inventory.
o	Handles loading and error states.
o	Fetches car data from the SQLM service.
8. ChooseCar.razor
•	Type: Blazor Component
•	Purpose:
o	Allows users to browse available cars for rent.
o	Displays car details and provides a "Rent" button for booking.
o	Navigates to the RentCar page for completing the booking process.
9. Counter.razor
•	Type: Blazor Component
•	Purpose: A simple counter component for testing and demonstration purposes.
10. Customers.razor
•	Type: Blazor Component
•	Purpose:
o	Displays a list of customers.
o	Provides a form for adding new customers.
o	Fetches and updates customer data using the SQLM service.
11. Home.razor
•	Type: Blazor Component
•	Purpose: Serves as the welcome page for the application, displaying a basic introduction.
12. Main.razor
•	Type: Blazor Component
•	Purpose:
o	Acts as the root component.
o	Allows users to select their role (Admin or User).
o	Displays the appropriate home page (AdminHome or UserHome) based on the selected role.
13. RentCar.razor
•	Type: Blazor Component
•	Purpose:
o	Handles the car rental process.
o	Accepts user details and books the car.
o	Updates the car's availability and saves the rental details to the database.
14. UserHome.razor
•	Type: Blazor Component
•	Purpose: Provides a simple user home page with a button to navigate to the ChooseCar page for renting a car.

