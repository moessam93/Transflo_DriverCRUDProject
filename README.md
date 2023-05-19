# Transflo_DriverCRUDProject

This project consists of a DataSeeder program and a set of APIs for managing drivers in a database. The DataSeeder program is responsible for populating the database with sample driver data, while the APIs provide endpoints for performing CRUD (Create, Read, Update, Delete) operations on the driver data.

## DataSeeder Program

The DataSeeder program is a standalone application that populates the database with sample driver data. It uses a connection string from the configuration file to establish a connection to the database and inserts randomly generated driver records into the "Drivers" table. The program is executed by running the `Main` method in the `Program` class.

To configure the DataSeeder program, you need to provide the database connection string in the `appsettings.json` file. The connection string should be specified under the key "DefaultConnection". Once configured, you can run the program by following the instructions provided during runtime.

## APIs

The APIs are built using ASP.NET Core and provide endpoints for managing driver records in the database. The APIs follow RESTful principles and support standard HTTP methods (GET, POST, PUT, DELETE) for performing operations on the driver data.

The following endpoints are available:

- GET /api/Driver/All: Retrieves a list of all drivers.
- GET /api/Driver/{id}: Retrieves a specific driver by their ID.
- GET /api/Driver/Alphapetized/{id}: Retrieves driver full name Alphapetized by their ID.
- POST /api/Driver: Creates a new driver record.
- PUT /api/Driver/{id}: Updates an existing driver record.
- DELETE /api/Driver/{id}: Deletes a driver record.

The APIs interact with the database using the provided connection string in the configuration file. The connection string should be specified under the key "DefaultConnection". Make sure to configure the connection string before running the APIs.

## Getting Started

To run the DataSeeder program and APIs, follow these steps:

1. Clone the repository to your local machine.
2. Configure the database connection string in the `appsettings.json` file.
3. Build the project and ensure all dependencies are resolved.
4. Run the DataSeeder program to populate the database with sample driver data.
5. Start the APIs and access them using the provided endpoints.

## Dependencies

The project has the following dependencies:

- Microsoft.Extensions.Configuration: Used for reading configuration settings.
- System.Data.SqlClient: Used for connecting to the SQL Server database.
- Microsoft.AspNetCore.Mvc: Used for building the APIs.

Make sure to restore these dependencies using NuGet before running the project.

## License

This project is licensed under the [MIT License](LICENSE).

