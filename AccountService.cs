using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using MySql.Data.MySqlClient;
using Microsoft.Data.SqlClient;

namespace OWASP_v1
{
    public class AccountService
    {
        private readonly string _connectionString = "Server=YOUR_SERVER;Database=YOUR_DB;Trusted_Connection=True;TrustServerCertificate=True;";

        /// <summary>
        /// VULNERABLE: Demonstrates how SQL Injection allows bypass or data exposure.
        /// DO NOT USE THIS IN PRODUCTION.
        /// </summary>
        /// <param name="input">If the input is: ' OR '1'='1 </param>
        public void VulnerableGetAccount(string input)
        {
            // VULNERABILITY: Raw string concatenation builds the query structure dynamically based on user input.
            string query = "SELECT * FROM accounts WHERE custID='" + input + "'";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // EXPLOIT RESULT: If input is: ' OR '1'='1
                            // The executed SQL becomes: SELECT * FROM accounts WHERE custID='' OR '1'='1'
                            // This bypasses the ID check and returns EVERY account in the database.
                            Console.WriteLine($"Vulnerable Read - CustID: {reader["custID"]}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// SECURE: Uses parameterized queries to neutralize SQL Injection.
        /// USE THIS APPROACH.
        /// </summary>
        /// <param name="input">Even if the input is: ' OR '1'='1 </param>
        public void SecureGetAccount(string input)
        {
            // FIX: Place a placeholder (@custID) instead of mixing input with SQL syntax
            string query = "SELECT * FROM accounts WHERE custID = @custID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // FIX: Pass the input as a strongly typed data parameter, not code text.
                command.Parameters.Add("@custID", SqlDbType.NVarChar).Value = input;

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // SECURITY RESULT: If input is: ' OR '1'='1
                            // The database literally looks for a user whose ID physically equals "' OR '1'='1".
                            // No records leak because the input cannot manipulate the logic of the SQL statement.
                            Console.WriteLine($"Secure Read - CustID: {reader["custID"]}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }

}
