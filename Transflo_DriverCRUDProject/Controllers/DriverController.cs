using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Data.SqlClient;
using Transflo_DriverCRUDProject.DTOs;
using Transflo_DriverCRUDProject.DTOs.Driver;
using Transflo_DriverCRUDProject.DTOs.Response;
using Transflo_DriverCRUDProject.Repos;

namespace Transflo_DriverCRUDProject.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class DriverController : ControllerBase
    {


        private readonly IConfiguration _configuration;

        public DriverController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public GetDriverDto GetDriverById(int id)
        {
            // Get the connection string from the configuration
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            var result = new GetDriverDto();

            using (SqlConnection connection = new(connectionString))
            {
                // Construct the SQL query string to retrieve the driver with the specified ID
                string query = $"SELECT * FROM Drivers WHERE Id = {id}";
                SqlCommand command = new(query, connection);

                // Open the database connection
                connection.Open();

                // Execute the SQL query and retrieve the results
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // Map the database columns to the properties of the GetDriverDto object
                    result.Id = (int)reader["Id"];
                    result.FirstName = (string)reader["FirstName"];
                    result.LastName = (string)reader["LastName"];
                    result.Email = (string)reader["Email"];
                    result.PhoneNumber = (string)reader["PhoneNumber"];
                }

                // Close the data reader
                reader.Close();
            }

            // Return the populated GetDriverDto object
            return result;
        }

        [HttpGet("All")]
        public IEnumerable<GetDriverDto> GetAll()
        {
            // Get the connection string from the configuration
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            var result = new List<GetDriverDto>();

            using (SqlConnection connection = new(connectionString))
            {
                // Define the SQL query to select all drivers
                string query = "SELECT * FROM Drivers";
                SqlCommand command = new(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // Map the database columns to the properties of the GetDriverDto object
                    var driver = new GetDriverDto
                    {
                        Id = (int)(reader["Id"]),
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"],
                        Email = (string)reader["Email"],
                        PhoneNumber = (string)reader["PhoneNumber"]
                    };

                    result.Add(driver);
                }

                reader.Close();
            }

            // Return the list of driver records
            return result;
        }


        [HttpGet("Alphapetized/{id}")]
        public string GetDriverNameAlphabetized(int id)
        {
            // Get the driver information by ID
            GetDriverDto driver = GetDriverById(id);

            if (driver != null)
            {
                // Convert the driver's first name to lowercase and store it in a character array
                char[] driverFirstName = driver.FirstName.ToLower().ToCharArray();
                Array.Sort(driverFirstName);

                // Iterate through each character in the driver's first name
                for (int i = 0; i < driverFirstName.Length; i++)
                {
                    // check if it exists in the original first name (uppercase)
                    if (driver.FirstName.IndexOf(char.ToUpper(driverFirstName[i])) != -1)
                    {
                        // Convert the character to uppercase
                        driverFirstName[i] = char.ToUpper(driverFirstName[i]);
                    }
                }

                // Convert the sorted and modified character array back to a string
                var alphabetizedFirstName = new String(driverFirstName);

                // Convert the driver's last name to lowercase and store it in a character array
                char[] driverLastName = driver.LastName.ToLower().ToCharArray();
                Array.Sort(driverLastName);

                // Iterate through each character in the driver's last name
                for (int i = 0; i < driverLastName.Length; i++)
                {

                    // check if it exists in the original last name (uppercase)
                    if (driver.LastName.IndexOf(char.ToUpper(driverLastName[i])) != -1)
                    {
                        // Convert the character to uppercase
                        driverLastName[i] = char.ToUpper(driverLastName[i]);
                    }
                }

                // Convert the sorted and modified character array back to a string
                var alphabetizedLastName = new String(driverLastName);

                // Return the alphabetized full name (first name + last name)
                return alphabetizedFirstName + " " + alphabetizedLastName;
            }
            else
            {
                // If the driver is not found, return null
                return $"No Driver with Id {id} exists";
            }
        }


        [HttpPost]
        public DriverResponse AddDriver(AddDriverDto driver)
        {
            // Get the connection string from the configuration
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using SqlConnection connection = new(connectionString);
            // Create SQL queries to check if a driver with the same phone number or email already exists
            string findByPhoneNumberQuery = $"SELECT * FROM Drivers WHERE PhoneNumber = '{driver.PhoneNumber}'";
            string findByEmailQuery = $"SELECT * FROM Drivers WHERE Email = '{driver.Email}'";

            // Create SQL commands for executing the queries
            SqlCommand findByPhoneNumberCommand = new(findByPhoneNumberQuery, connection);
            SqlCommand findByEmailCommand = new(findByEmailQuery, connection);

            connection.Open();

            // Execute the query to check if a driver with the same phone number exists
            SqlDataReader readerPhoneNumber = findByPhoneNumberCommand.ExecuteReader();

            if (readerPhoneNumber.Read() && (int)(readerPhoneNumber["Id"]) > 0)
            {
                // If a driver with the same phone number exists, return a response indicating the duplicate
                return new DriverResponse()
                {
                    Id = null,
                    Message = $"Driver with Phone Number {driver.PhoneNumber} already exists"
                };
            }

            // Execute the query to check if a driver with the same email exists
            SqlDataReader readerEmail = findByEmailCommand.ExecuteReader();

            if (readerEmail.Read() && (int)(readerEmail["Id"]) > 0)
            {
                // If a driver with the same email exists, return a response indicating the duplicate
                return new DriverResponse()
                {
                    Id = null,
                    Message = $"Driver with Email {driver.Email} already exists"
                };
            }

            // Validate the driver information
            if (string.IsNullOrEmpty(driver.FirstName?.Trim()))
            {
                // Return a response indicating that the driver's first name is null or empty
                return new DriverResponse()
                {
                    Id = null,
                    Message = $"Driver's First Name can't be null or empty"
                };
            }
            if (string.IsNullOrEmpty(driver.LastName?.Trim()))
            {
                // Return a response indicating that the driver's last name is null or empty
                return new DriverResponse()
                {
                    Id = null,
                    Message = $"Driver's Last Name can't be null or empty"
                };
            }
            if (string.IsNullOrEmpty(driver.Email?.Trim()))
            {
                // Return a response indicating that the driver's email is null or empty
                return new DriverResponse()
                {
                    Id = null,
                    Message = $"Driver's Email can't be null or empty"
                };
            }
            if (string.IsNullOrEmpty(driver.PhoneNumber?.Trim()))
            {
                // Return a response indicating that the driver's phone number is null or empty
                return new DriverResponse()
                {
                    Id = null,
                    Message = $"Driver's Phone Number can't be null or empty"
                };
            }

            // If all validation checks pass, execute the query to insert the new driver
            string insertQuery = $"INSERT INTO Drivers (FirstName,LastName,Email,PhoneNumber) OUTPUT INSERTED.Id values ('{driver.FirstName}','{driver.LastName}','{driver.Email}','{driver.PhoneNumber}')";
            SqlCommand insertCommand = new(insertQuery, connection);
            // Execute the insert command and retrieve the new driver's ID
            int newDriverId = (int)insertCommand.ExecuteScalar();

            if (newDriverId > 0)
            {
                // Return a successful response with the new driver's ID
                return new DriverResponse()
                {
                    Id = newDriverId,
                    Message = "Driver has been added successfully"
                };
            }
            else
            {
                // If the insert fails, return a response indicating the failure
                return new DriverResponse()
                {
                    Id = null,
                    Message = "Failed to add driver"
                };
            }
        }


        [HttpPut("{id}")]
        public DriverResponse UpdateDriver(int id, UpdateDriverDto driver)
        {
            // Get the connection string from the configuration
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using SqlConnection connection = new(connectionString);
            // Create SQL queries to find the driver by ID, phone number, and email
            string findDriverByIdQuery = $"SELECT * FROM Drivers WHERE Id = {id}";
            string findByPhoneNumberQuery = $"SELECT * FROM Drivers WHERE PhoneNumber = '{driver.PhoneNumber}'";
            string findByEmailQuery = $"SELECT * FROM Drivers WHERE Email = '{driver.Email}'";

            // Create SQL commands for executing the queries
            SqlCommand findDriverByIdCommand = new(findDriverByIdQuery, connection);
            SqlCommand findByPhoneNumberCommand = new(findByPhoneNumberQuery, connection);
            SqlCommand findByEmailCommand = new(findByEmailQuery, connection);

            connection.Open();

            // Execute the query to find the driver by ID
            SqlDataReader readerId = findDriverByIdCommand.ExecuteReader();
            if (!readerId.Read())
            {
                // If no driver with the given ID exists, return a response indicating the absence
                return new DriverResponse()
                {
                    Id = null,
                    Message = $"No Driver with Id {id} exists"
                };
            }

            // Execute the query to check if a driver with the same phone number exists
            SqlDataReader readerPhoneNumber = findByPhoneNumberCommand.ExecuteReader();
            if (readerPhoneNumber.Read() && (int)(readerPhoneNumber["Id"]) != id)
            {
                // If a driver with the same phone number is found (excluding the current driver being updated), return a response indicating the duplicate
                return new DriverResponse()
                {
                    Id = null,
                    Message = $"Driver with Phone Number {driver.PhoneNumber} already exists"
                };
            }

            // Execute the query to check if a driver with the same email exists
            SqlDataReader readerEmail = findByEmailCommand.ExecuteReader();
            if (readerEmail.Read() && (int)(readerEmail["Id"]) != id)
            {
                // If a driver with the same email is found (excluding the current driver being updated), return a response indicating the duplicate

                return new DriverResponse()
                {
                    Id = null,
                    Message = $"Driver with Email {driver.Email} already exists"
                };

            }

            // Create an update query to update the driver's information in the database
            string updateQuery = "UPDATE Drivers SET ";

            //Ignore null and empty fields
            if (!string.IsNullOrEmpty(driver.FirstName?.Trim()))
            {
                updateQuery += $"FirstName = '{driver.FirstName}',";

            }
            if (!string.IsNullOrEmpty(driver.LastName?.Trim()))
            {
                updateQuery += $"LastName = '{driver.LastName}',";

            }
            if (!string.IsNullOrEmpty(driver.Email?.Trim()))
            {
                updateQuery += $"Email = '{driver.Email}',";

            }
            if (!string.IsNullOrEmpty(driver.PhoneNumber?.Trim()))
            {
                updateQuery += $"PhoneNumber = '{driver.PhoneNumber}',";

            }

            // Remove the trailing comma
            updateQuery = updateQuery.TrimEnd(',');

            // Add the condition for the driver's ID
            updateQuery += $" WHERE Id = {id}";

            SqlCommand updateCommand = new(updateQuery, connection);

            // Execute the update command and get the number of rows affected
            int rowsAffected = updateCommand.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                // If the update is successful, return a response indicating the success
                return new DriverResponse()
                {
                    Id = id,
                    Message = "Driver has been updated successfully"
                };
            }
            else
            {
                // If the update fails, return a response indicating the failure
                return new DriverResponse()
                {
                    Id = null,
                    Message = "Failed to update driver"
                };
            }
        }


        [HttpDelete("{id}")]
        public DriverResponse DeleteDriver(int id)
        {
            // Get the connection string from the configuration
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using SqlConnection connection = new(connectionString);
            // Create SQL query to find the driver by ID
            string findDriverByIdQuery = $"SELECT * FROM Drivers WHERE Id = {id}";

            // Create SQL command for executing the query
            SqlCommand findDriverByIdCommand = new(findDriverByIdQuery, connection);
            connection.Open();

            // Execute the query to find the driver by ID
            SqlDataReader readerId = findDriverByIdCommand.ExecuteReader();
            if (!readerId.Read())
            {
                // If no driver with the given ID exists, return a response indicating the absence
                return new DriverResponse()
                {
                    Id = null,
                    Message = $"No Driver with Id {id} exists"
                };
            }

            // Create a delete query to delete the driver from the database
            string deleteQuery = $"DELETE FROM Drivers where Id = {id}";
            SqlCommand deleteCommand = new(deleteQuery, connection);
            // Execute the delete command and get the number of rows affected
            int rowsAffected = deleteCommand.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                // If the delete is successful, return a response indicating the success
                return new DriverResponse()
                {
                    Id = id,
                    Message = "Driver has been deleted successfully"
                };
            }
            else
            {
                // If the delete fails, return a response indicating the failure
                return new DriverResponse()
                {
                    Id = null,
                    Message = "Failed to delete driver"
                };
            }
        }

    }
}