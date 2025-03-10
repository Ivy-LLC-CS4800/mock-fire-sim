using UnityEngine;
using Mono.Data.Sqlite;  // SQLite support
using UnityEngine.UI;
using System.Data;  // To access SQLite commands

public class DatabaseManager : MonoBehaviour
{
    public InputField usernameInputField;
    public Button loginButton;
    public Button registerButton;

    private string dbPath = "URI=file:users.db";

    void Start()
    {
        CreateDB();
        // Add button listeners
        loginButton.onClick.AddListener(OnLoginButtonClick);
        registerButton.onClick.AddListener(OnRegisterButtonClick);
    }

    public void CreateDB()
    {
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS users (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL UNIQUE);";
                command.ExecuteNonQuery();
                Debug.Log("table create");
            }
            connection.Close();
        }
    }

    // Login button click event
    public void OnLoginButtonClick()
    {
        string username = usernameInputField.text;
        if (string.IsNullOrEmpty(username))
        {
            return;
        }

        // Check if the username exists in the database
        if (CheckUsernameExists(username))
        {
            Debug.Log("Login successful for: " + username);
            SceneLoader sl = new SceneLoader();
            sl.LoadMainScene();
            Global.GlobalUser = username;
        }
        else
        {
            Debug.Log("Username does not exist.");
        }
    }

    // Register button click event
    public void OnRegisterButtonClick()
    {
        string username = usernameInputField.text;
        if (string.IsNullOrEmpty(username))
        {
            return;
        }

        // Register the new username in the database
        if (RegisterUsername(username))
        {
            Debug.Log("User registered: " + username);
        }
        else
        {
            Debug.Log("Username already exists.");
        }
    }

    // Check if the username exists in the database
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

    // Register a new username
    bool RegisterUsername(string username)
    {
        // Check if the username already exists
        if (CheckUsernameExists(username))
        {
            return false; // Username already exists
        }

        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO users (name) VALUES (@name)";
                command.Parameters.Add(new SqliteParameter("@name", username));
                command.ExecuteNonQuery();
                connection.Close();

                return true; // Registration successful
            }
        }
    }
}
