using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CarRentalSystem.Backend.Models;
namespace CarRentalSystem
{
    // SQL Manager class for managing database interactions
    public class SQLM
    {
        // Connection string for connecting to the database
        private readonly string _connectionString =
            @"Data Source=localhost\SQLEXPRESS02;Initial Catalog=CarRentalSystem;Integrated Security=True;User ID=sa;Password=demol23";

        // Constructor to allow custom connection string (currently commented out)
        public SQLM(string connectionString)
        {
            // _connectionString = connectionString; // Allows overriding the default connection string
        }

        // Fetches all cars from the database
        public List<Car> GetAllCars()
        {
            var cars = new List<Car>();
            string query = "SELECT Id, Name, Year, PricePerDay, IsAvailable FROM Cars";

            try
            {
                using (var connection = new SqlConnection(_connectionString)) // Opens a database connection
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection)) // Prepares the SQL query
                    {
                        using (var reader = command.ExecuteReader()) // Executes the query and reads results
                        {
                            while (reader.Read()) // Iterates through the result set
                            {
                                cars.Add(new Car
                                {
                                    Id = reader.GetInt32(0), // Maps the first column to the Car ID
                                    Name = reader.GetString(1), // Maps the second column to the Car Name
                                    Year = reader.GetInt32(2), // Maps the third column to the Year
                                    PricePerDay = reader.GetDecimal(3), // Maps the fourth column to the PricePerDay
                                    IsAvailable = reader.GetBoolean(4) // Maps the fifth column to IsAvailable
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching cars: {ex.Message}"); // Logs any errors
                throw; // Re-throws the exception for upstream handling
            }

            return cars; // Returns the list of cars
        }

        // Fetches all customers asynchronously
        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            var customers = new List<Customer>();
            string query = "SELECT Id, Name, Phone FROM Customers";

            try
            {
                using (var connection = new SqlConnection(_connectionString)) // Opens a database connection
                {
                    await connection.OpenAsync(); // Opens the connection asynchronously
                    using (var command = new SqlCommand(query, connection)) // Prepares the SQL query
                    {
                        using (var reader = await command.ExecuteReaderAsync()) // Executes the query and reads results asynchronously
                        {
                            while (await reader.ReadAsync()) // Iterates through the result set
                            {
                                customers.Add(new Customer
                                {
                                    Id = reader.GetInt32(0), // Maps the first column to the Customer ID
                                    Name = reader.GetString(1), // Maps the second column to the Customer Name
                                    Phone = reader.GetString(2) // Maps the third column to the Customer Phone
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching customers: {ex.Message}"); // Logs any errors
                throw; // Re-throws the exception for upstream handling
            }

            return customers; // Returns the list of customers
        }

        // Fetches all rentals asynchronously, including car and customer details
        public async Task<List<Rental>> GetAllRentalsAsync()
        {
            var rentals = new List<Rental>();
            string query = @"SELECT r.Id, r.CarId, r.CustomerId, r.RentalDate, 
                                    c.Name AS CarName, cu.Name AS CustomerName
                             FROM Rentals r
                             JOIN Cars c ON r.CarId = c.Id
                             JOIN Customers cu ON r.CustomerId = cu.Id";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                rentals.Add(new Rental
                                {
                                    Id = reader.GetInt32(0), // Maps Rental ID
                                    CarId = reader.GetInt32(1), // Maps Car ID
                                    CustomerId = reader.GetInt32(2), // Maps Customer ID
                                    RentalDate = reader.GetDateTime(3), // Maps Rental Date
                                    CarName = reader.GetString(4), // Maps Car Name
                                    CustomerName = reader.GetString(5) // Maps Customer Name
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching rentals: {ex.Message}");
                throw;
            }

            return rentals; // Returns the list of rentals
        }

        // Adds a new car to the database
        public async Task AddCarAsync(string name, int year, decimal pricePerDay, bool isAvailable)
        {
            string query = "INSERT INTO Cars (Name, Year, PricePerDay, IsAvailable) VALUES (@Name, @Year, @PricePerDay, @IsAvailable)";
            await ExecuteNonQueryAsync(query, new Dictionary<string, object>
    {
        { "@Name", name },
        { "@Year", year },
        { "@PricePerDay", pricePerDay },
        { "@IsAvailable", isAvailable }
    });
        }


        // Adds a new customer to the database and returns the generated customer ID
        public async Task<int> AddCustomerAsync(string name, string phone)
        {
            string query = "INSERT INTO Customers (Name, Phone) OUTPUT INSERTED.Id VALUES (@Name, @Phone)";
            int customerId;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Phone", phone);

                        customerId = (int)await command.ExecuteScalarAsync(); // Retrieves the generated ID
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding customer: {ex.Message}");
                throw;
            }

            return customerId;
        }

        // Adds a new rental to the database
        public async Task AddRentalAsync(int carId, int customerId, DateTime rentalDate)
        {
            string query = "INSERT INTO Rentals (CarId, CustomerId, RentalDate) VALUES (@CarId, @CustomerId, @RentalDate)";

            try
            {
                await ExecuteNonQueryAsync(query, new Dictionary<string, object>
                {
                    { "@CarId", carId },
                    { "@CustomerId", customerId },
                    { "@RentalDate", rentalDate }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding rental: {ex.Message}");
                throw;
            }
        }

        // Helper method for executing non-query SQL commands
        private async Task ExecuteNonQueryAsync(string query, Dictionary<string, object> parameters)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(query, connection))
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value); // Binds parameters
                        }

                        await command.ExecuteNonQueryAsync(); // Executes the query
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing query: {ex.Message}");
                throw;
            }
        }
        // Checks if a car is available for rent
        public async Task<bool> IsCarAvailableAsync(int carId)
        {
            string query = "SELECT IsAvailable FROM Cars WHERE Id = @CarId";
            bool isAvailable = false;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CarId", carId);
                        var result = await command.ExecuteScalarAsync();
                        if (result != null)
                            isAvailable = Convert.ToBoolean(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking car availability: {ex.Message}");
                throw;
            }

            return isAvailable;
        }
        // Updates the availability of a car
        public async Task UpdateCarAvailabilityAsync(int carId, bool isAvailable)
        {
            string query = "UPDATE Cars SET IsAvailable = @IsAvailable WHERE Id = @CarId";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IsAvailable", isAvailable);
                        command.Parameters.AddWithValue("@CarId", carId);
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating car availability: {ex.Message}");
                throw;
            }
        }
        public async Task UpdateCustomerAsync(int customerId, string name, string phone)
        {
            string query = "UPDATE Customers SET Name = @Name, Phone = @Phone WHERE Id = @CustomerId";

            try
            {
                await ExecuteNonQueryAsync(query, new Dictionary<string, object>
        {
            { "@CustomerId", customerId },
            { "@Name", name },
            { "@Phone", phone }
        });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating customer: {ex.Message}");
                throw;
            }
        }
        public async Task DeleteCustomerAsync(int customerId)
        {
            string query = "DELETE FROM Customers WHERE Id = @CustomerId";

            try
            {
                await ExecuteNonQueryAsync(query, new Dictionary<string, object>
        {
            { "@CustomerId", customerId }
        });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting customer: {ex.Message}");
                throw;
            }
        }
        public async Task UpdateCarAsync(int id, string name, int year, decimal pricePerDay, bool isAvailable)
        {
            string query = "UPDATE Cars SET Name = @Name, Year = @Year, PricePerDay = @PricePerDay, IsAvailable = @IsAvailable WHERE Id = @Id";
            await ExecuteNonQueryAsync(query, new Dictionary<string, object>
    {
        { "@Id", id },
        { "@Name", name },
        { "@Year", year },
        { "@PricePerDay", pricePerDay },
        { "@IsAvailable", isAvailable }
    });
        }
        public async Task DeleteCarAsync(int id)
        {
            string query = "DELETE FROM Cars WHERE Id = @Id";
            await ExecuteNonQueryAsync(query, new Dictionary<string, object>
    {
        { "@Id", id }
    });
        }

    }
}
