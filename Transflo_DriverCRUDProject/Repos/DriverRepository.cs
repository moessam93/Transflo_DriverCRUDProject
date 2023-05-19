using System;
using System.Data;
using System.Data.SqlClient;
namespace Transflo_DriverCRUDProject.Repos
{
    public class DriverRepository : IDisposable
    {
        private readonly string connectionString;

        public DriverRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            InitializeDatabase();
        }

        public void InitializeDatabase()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();


                SqlCommand command = new SqlCommand();

                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Drivers')
                                        BEGIN
                                            CREATE TABLE Drivers (
                                            Id INT IDENTITY(1,1) PRIMARY KEY,
                                            FirstName NVARCHAR(450) NOT NULL,
                                            LastName NVARCHAR(450) NOT NULL,
                                            Email NVARCHAR(100) NOT NULL UNIQUE,
                                            PhoneNumber NVARCHAR(100) NOT NULL UNIQUE
                                        )
                                        END";

                int rowsAffected = command.ExecuteNonQuery();

            }

            Console.WriteLine("Drivers Table Created Successfully initialized successfully.");
        }

        public void Dispose()
        {
            // Clean up resources if needed
        }
    }
}

