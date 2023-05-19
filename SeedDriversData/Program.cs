using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;


class Program
{
    private static string connectionString;

    static void Main(string[] args)
    {

        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        connectionString = configuration.GetConnectionString("DefaultConnection");

        Console.WriteLine("Welcome to the Data Seeding Program!");

        Console.WriteLine("Press 1 and Enter to seed the data...");
        string userInput = Console.ReadLine();

        if (userInput == "1")
        {
            try
            {
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
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            SqlCommand command = new SqlCommand();

            command.Connection = connection;
            command.CommandType = CommandType.Text;
            command.CommandText = @"WITH Numbers AS (
    SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNumber
    FROM sys.all_objects
),
LastId AS (
    SELECT MAX(Id) AS LastId
    FROM Drivers
)
INSERT INTO Drivers (FirstName, LastName, Email, PhoneNumber)
SELECT
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
    CONCAT(CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) +
    CHAR(97 + (ABS(CHECKSUM(NEWID())) % 26)) , '@email.com') AS Email,
    CAST(ABS(CHECKSUM(NEWID())) AS INT) AS PhoneNumber
FROM Numbers
WHERE RowNumber <= 100;";

            int rowsAffected = command.ExecuteNonQuery();
        }

    }
}
