using UnityEngine;
using Mono.Data.Sqlite;  // SQLite support
using UnityEngine.UI;
using System.Data;  // To access SQLite commands
using System.Security.Cryptography;
using System.Text;  // For password hashing

//<summary>
// Creates a database instance and connects it to the local device database to check usernames,passwords, and register new users
//</summary>
public class DatabaseManager : MonoBehaviour
{
    private string dbPath = "URI=file:users.db";

    private SqliteConnection connection;

    //TODO: Initialize database
    //Parameters: Start application
    void Start()
    {
        CreateDB();
    }

    public void SetConnection(SqliteConnection conn)
    {
        connection = conn;
    }

    //TODO: Creates a database instance and connects to local device database
    //Parameters:
    public void CreateDB()
    {
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // Create table with username and password fields
                command.CommandText = "CREATE TABLE IF NOT EXISTS users (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL UNIQUE, password TEXT NOT NULL);";
                command.ExecuteNonQuery();
                Debug.Log("Table created");
            }
            connection.Close();
        }
    }

    //TODO: Verifies if user exists in database and the password is correct
    //Parameters: Username, password, initialized database
    public bool CheckUsernameAndPassword(string username, string password)
    {
        return CheckUsernameAndPasswordHelper(username, password);
    }

    //TODO: Checks username and password against database
    //Parameters: Username, password, initialized database instance
    private bool CheckUsernameAndPasswordHelper(string username, string password)
    {
        string hashedPassword = HashPassword(password);
        //string hashedPassword = password; 

        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM users WHERE name = @name AND password = @password";
                command.Parameters.Add(new SqliteParameter("@name", username));
                command.Parameters.Add(new SqliteParameter("@password", hashedPassword));

                long result = (long)command.ExecuteScalar();
                connection.Close();

                return result > 0; // Returns true if username and password match, false otherwise
            }
        }
    }

    //TODO: Adds new username and password database
    //Parameters: Username, password, initialized database instance
    public virtual bool RegisterUsername(string username, string password)
    {
        return RegisterUsernameHelper(username, password);
    }

    //TODO: Adds new username and password database
    //Parameters: Username, password, initialized database instance
    private bool RegisterUsernameHelper(string username, string password)
    {
        // Check if the username already exists
        if (CheckUsernameExists(username))
        {
            return false; // Username already exists
        }

        string hashedPassword = HashPassword(password);
        //string hashedPassword = password;

        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO users (name, password) VALUES (@name, @password)";
                command.Parameters.Add(new SqliteParameter("@name", username));
                command.Parameters.Add(new SqliteParameter("@password", hashedPassword));
                command.ExecuteNonQuery();
                connection.Close();

                return true; // Registration successful
            }
        }
    }

    //TODO: Checks for existing username in database
    //Parameters: Username, initialized database instance
    bool CheckUsernameExists(string username)
    {
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM users WHERE name = @name";
                command.Parameters.Add(new SqliteParameter("@name", username));

                long result = (long)command.ExecuteScalar();
                connection.Close();

                return result > 0; // Returns true if username exists, false otherwise
            }
        }
    }

    //TODO: Encrpts password using SHA-256
    //Parameters: Password, initialized database instance
    string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString();
        }
    }
}
