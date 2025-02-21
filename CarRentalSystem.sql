-- Create Database
CREATE DATABASE CarRentalSystem;

-- Use the Database
USE CarRentalSystem;

-- Create Cars Table
CREATE TABLE Cars (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Year INT NOT NULL,
    PricePerDay DECIMAL(10, 2) NOT NULL,
    IsAvailable BIT NOT NULL DEFAULT 1
);

-- Create Customers Table
CREATE TABLE Customers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(15) NOT NULL
);

-- Create Rentals Table
CREATE TABLE Rentals (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CarId INT NOT NULL,
    CustomerId INT NOT NULL,
    RentalDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CarId) REFERENCES Cars(Id),
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);

-- Seed Initial Data for Cars
INSERT INTO Cars (Name, Year, PricePerDay, IsAvailable) VALUES
('Toyota Corolla', 2020, 50.00, 0),
('Honda Civic', 2019, 60.00, 0),
('Ford Mustang', 2021, 120.00, 1),
('Chevrolet Malibu', 2020, 55.00, 1),
('Nissan Altima', 2018, 45.00, 1),
('BMW 3 Series', 2022, 150.00, 0),
('Mercedes C-Class', 2021, 180.00, 1);


-- Seed Initial Data for Customers
INSERT INTO Customers (Name, Phone) VALUES
('John Doe', '1234567890'),
('Jane Smith', '9876543210');
