using dz_3_in_1_dotnet;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Drawing;
using System.Runtime.Intrinsics.X86;
using System.Security.Principal;

class Program
{
    static void Main(string[] args)
    {
        //dz1
        using (var context = new dz1Context())
        {
            var files = context.Files.ToList();

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string folderPath = Path.Combine(desktopPath, "SavedFiles");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            foreach (var file in files)
            {
                string filePath = Path.Combine(folderPath, file.FileName);
                File.WriteAllBytes(filePath, file.Data);

                Console.WriteLine($"File {file.FileName} saved to {folderPath}");
            }
        }
        //dz2
        using (var context = new dz2Context())
        {
            bool addingUsers = true;

            while (addingUsers)
            {
                Console.Write("Name: ");
                string name = Console.ReadLine();

                Console.Write("Age: ");
                if (!int.TryParse(Console.ReadLine(), out int age))
                {
                    Console.WriteLine("Age format is incorrect.");
                    continue;
                }

                var user = new User { Name = name, Age = age };
                context.Users.Add(user);
                context.SaveChanges();

                Console.WriteLine("Success!");

                Console.Write("Add again? (yes/no): ");
                string answer = Console.ReadLine().ToLower();
                if (answer != "yes")
                {
                    addingUsers = false;
                }
            }

            int userCount = context.Users.Count();
            double averageAge = context.Users.Average(u => u.Age);

            Console.WriteLine($"User count: {userCount}");
            Console.WriteLine($"Average age: {averageAge:F2}");
        }
        //dz3
        // Tak vuglyadaye db:
        //CREATE DATABASE dz3dot;

        //USE dz3dot;

        //CREATE TABLE Products(
        //    Id INT PRIMARY KEY IDENTITY,
        //    Name NVARCHAR(100) NOT NULL,
        //    Type NVARCHAR(50) NOT NULL,
        //    Color NVARCHAR(50) NOT NULL,
        //    Calories INT NOT NULL
        //);
        string connectionString = "Server = (localdb)\\mssqllocaldb;Database = dz3dot;Trusted_Connection=true;";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Success.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                connection.Close();
                Console.WriteLine("Closed");
            }
        }
    }
    static void ShowAllProducts(SqlConnection connection)
    {
        string query = "SELECT * FROM Products";
        using (SqlCommand command = new SqlCommand(query, connection))
        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                Console.WriteLine($"Name: {reader["Name"]}, Type: {reader["Type"]}, Color: {reader["Color"]}, Calories: {reader["Calories"]}");
            }
        }
    }

    static void ShowAllNames(SqlConnection connection)
    {
        string query = "SELECT Name FROM Products";
        using (SqlCommand command = new SqlCommand(query, connection))
        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                Console.WriteLine($"Name: {reader["Name"]}");
            }
        }
    }

    static void ShowMaxCalories(SqlConnection connection)
    {
        string query = "SELECT MAX(Calories) FROM Products";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            int maxCalories = (int)command.ExecuteScalar();
            Console.WriteLine($"Max calories: {maxCalories}");
        }
    }

    static void ShowMinCalories(SqlConnection connection)
    {
        string query = "SELECT MIN(Calories) FROM Products";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            int minCalories = (int)command.ExecuteScalar();
            Console.WriteLine($"Min calories: {minCalories}");
        }
    }

    static void ShowAverageCalories(SqlConnection connection)
    {
        string query = "SELECT AVG(Calories) FROM Products";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            double avgCalories = (double)command.ExecuteScalar();
            Console.WriteLine($"Average calories: {avgCalories:F2}");
        }
    }

    static void ShowVegetableCount(SqlConnection connection)
    {
        string query = "SELECT COUNT(*) FROM Products WHERE Type = 'Vegetable'";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            int count = (int)command.ExecuteScalar();
            Console.WriteLine($"Count: {count}");
        }
    }

    static void ShowFruitCount(SqlConnection connection)
    {
        string query = "SELECT COUNT(*) FROM Products WHERE Type = 'Fruit'";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            int count = (int)command.ExecuteScalar();
            Console.WriteLine($"Count: {count}");
        }
    }

    static void ShowCountByColor(SqlConnection connection, string color)
    {
        string query = "SELECT COUNT(*) FROM Products WHERE Color = @color";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@color", color);
            int count = (int)command.ExecuteScalar();
            Console.WriteLine($"Count {color}: {count}");
        }
    }

    static void ShowCountByEachColor(SqlConnection connection)
    {
        string query = "SELECT Color, COUNT(*) AS Count FROM Products GROUP BY Color";
        using (SqlCommand command = new SqlCommand(query, connection))
        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                Console.WriteLine($"Color: {reader["Color"]}, Count: {reader["Count"]}");
            }
        }
    }

    static void ShowProductsByCaloriesLessThan(SqlConnection connection, int maxCalories)
    {
        string query = "SELECT * FROM Products WHERE Calories < @maxCalories";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@maxCalories", maxCalories);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Name: {reader["Name"]}, Type: {reader["Type"]}, Color: {reader["Color"]}, Calories: {reader["Calories"]}");
                }
            }
        }
    }

    static void ShowProductsByCaloriesGreaterThan(SqlConnection connection, int minCalories)
    {
        string query = "SELECT * FROM Products WHERE Calories > @minCalories";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@minCalories", minCalories);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Name: {reader["Name"]}, Type: {reader["Type"]}, Color: {reader["Color"]}, Calories: {reader["Calories"]}");
                }
            }
        }
    }

    static void ShowProductsByCaloriesRange(SqlConnection connection, int minCalories, int maxCalories)
    {
        string query = "SELECT * FROM Products WHERE Calories BETWEEN @minCalories AND @maxCalories";
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@minCalories", minCalories);
            command.Parameters.AddWithValue("@maxCalories", maxCalories);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Name: {reader["Name"]}, Type: {reader["Type"]}, Color: {reader["Color"]}, Calories: {reader["Calories"]}");
                }
            }
        }
    }

    static void ShowYellowOrRedProducts(SqlConnection connection)
    {
        string query = "SELECT * FROM Products WHERE Color = 'Yellow' OR Color = 'Red'";
        using (SqlCommand command = new SqlCommand(query, connection))
        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                Console.WriteLine($"Name: {reader["Name"]}, Type: {reader["Type"]}, Color: {reader["Color"]}, Calories: {reader["Calories"]}");
            }
        }
    }

}
