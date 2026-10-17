using PR5.Model;    
using PR5.Security; 
using System;
using System.Data.SqlClient;

namespace PR5
{
    internal class Program
    {
       
        private static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Bakery;Integrated Security=True;";

        static void Main(string[] args)
        {
            Console.WriteLine("Создание новой учетной записи для пользователя\n");

          
            Console.Write("Введите имя пользователя: ");
            string firstName = Console.ReadLine();

            Console.Write("Введите фамилию пользователя: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите логин пользователя: ");
            string login = Console.ReadLine();

            Console.Write("Введите пароль пользователя: ");
            string password = Console.ReadLine();

           
            PasswordHasher hasher = new PasswordHasher();
            string passwordHash = hasher.GetHash(password);

            Console.WriteLine($"Хешированный пароль пользователя: {passwordHash}");

            
            User newUser = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Login = login,
                PasswordHash = passwordHash
            };

          
            try
            {
                AddUserToDatabase(newUser);
                Console.WriteLine("Учетная запись добавлена");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении: {ex.Message}");
            }

            Console.ReadLine();
        }

        
        static void AddUserToDatabase(User user)
        {
            string query = @"INSERT INTO Users (FirstName, LastName, Login, PasswordHash) 
                             VALUES (@FirstName, @LastName, @Login, @PasswordHash);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@Login", user.Login);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}