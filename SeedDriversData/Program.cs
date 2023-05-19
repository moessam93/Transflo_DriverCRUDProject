using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Numerics;

class Program
{
    private static string connectionString;

    static void Main(string[] args)
    {
        // Configuring the application settings using the appsettings.json file
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        // Retrieving the connection string from the configuration
        connectionString = configuration.GetConnectionString("DefaultConnection");

        Console.WriteLine("Welcome to the Data Seeding Program!");

        Console.WriteLine("Press 1 and Enter to seed the data...");
        string userInput = Console.ReadLine();

        if (userInput == "1")
        {
            try
            {
                // Calling the SeedDrivers method to seed data into the database
                SeedDrivers();

                Console.WriteLine("Data seeding operation completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Data seeding operation failed: " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Data seeding operation canceled.");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    public static void SeedDrivers()
    {
        // Establishing a connection to the database using the connection string
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // Creating a SQL command object to execute the data seeding query
            SqlCommand command = new SqlCommand();

            command.Connection = connection;
            command.CommandType = CommandType.Text;

            // Setting the data seeding query to insert random driver data into the Drivers table
            command.CommandText = @"
                WITH Numbers AS (
                    SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNumber
                    FROM sys.all_objects
                )
                INSERT INTO Drivers (FirstName, LastName, Email, PhoneNumber)
                SELECT
                    -- Generating a random first name
                    CHAR(65 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) AS FirstName,

                    -- Generating a random last name
                    CHAR(65 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) AS LastName,

                    -- Generating an email in the format randomString@transflo.com
                    CONCAT(CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
                    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) , '@transflo.com') As Email,

                    --Generating a random phone number
                    CAST(ABS(CHECKSUM(NEWID())) AS INT) AS PhoneNumber
                FROM Numbers
                WHERE RowNumber <= 100;";

            // Executing the data seeding query and returning the number of rows affected
            int rowsAffected = command.ExecuteNonQuery();
        }
    }
}
