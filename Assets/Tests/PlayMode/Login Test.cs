using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System.Collections;
using TMPro;
using UnityEngine.TestTools;

public class LoginTest
{
    private GameObject testObject;
    private Login login;
    private string connectionString = "URI=file::memory:"; // In-memory SQLite database
    private SqliteConnection connection;

    [SetUp]
    public void SetUp()
    {
        // Create a GameObject and attach the Login component
        testObject = new GameObject("LoginTestObject");
        login = testObject.AddComponent<Login>();
        Assert.IsNotNull(login, "Login component should not be null.");

        // Mock UI elements
        login.usernameInputField = new GameObject("UsernameInputField").AddComponent<InputField>();
        login.passwordInputField = new GameObject("PasswordInputField").AddComponent<InputField>();
        login.loginButton = new GameObject("LoginButton").AddComponent<Button>();
        login.registerButton = new GameObject("RegisterButton").AddComponent<Button>();
        Assert.IsNotNull(login.usernameInputField, "UsernameInputField should not be null.");
        Assert.IsNotNull(login.passwordInputField, "PasswordInputField should not be null.");
        Assert.IsNotNull(login.loginButton, "LoginButton should not be null.");
        Assert.IsNotNull(login.registerButton, "RegisterButton should not be null.");

        // Mock dependencies
        login.sceneCall = testObject.AddComponent<SceneLoader>(); // Use AddComponent instead of new
        Assert.IsNotNull(login.sceneCall, "SceneLoader should not be null.");

        // Initialize Notification components
        var errorNotificationObject = new GameObject("ErrorNotification");
        var errorPopup = new GameObject("ErrorPopup");
        errorPopup.AddComponent<Canvas>(); // Add a Canvas component to simulate UI behavior
        var errorText = new GameObject("ErrorText").AddComponent<TextMeshProUGUI>();
        errorPopup.transform.SetParent(errorNotificationObject.transform);

        var errorNotification = errorNotificationObject.AddComponent<Notification>();
        errorNotification.notificationPopup = errorPopup;
        errorNotification.notificationText = errorText;
        login.errorCall = errorNotification;
        Assert.IsNotNull(login.errorCall, "Error Notification should not be null.");

        var successNotificationObject = new GameObject("SuccessNotification");
        var successPopup = new GameObject("SuccessPopup");
        successPopup.AddComponent<Canvas>(); // Add a Canvas component to simulate UI behavior
        var successText = new GameObject("SuccessText").AddComponent<TextMeshProUGUI>();
        successPopup.transform.SetParent(successNotificationObject.transform);

        var successNotification = successNotificationObject.AddComponent<Notification>();
        successNotification.notificationPopup = successPopup;
        successNotification.notificationText = successText;
        login.successCall = successNotification;
        Assert.IsNotNull(login.successCall, "Success Notification should not be null.");

        // Set up the in-memory SQLite database
        connection = new SqliteConnection(connectionString);
        Assert.IsNotNull(connection, "SqliteConnection should not be null.");
        connection.Open();
        Assert.IsTrue(connection.State == ConnectionState.Open, "SqliteConnection should be open.");

        using (var cmd = connection.CreateCommand())
        {
            // Create the users table
            cmd.CommandText = "CREATE TABLE users (username TEXT, password TEXT)";
            cmd.ExecuteNonQuery();

            // Insert mock data
            cmd.CommandText = "INSERT INTO users (username, password) VALUES ('TestUser', 'TestPassword')";
            cmd.ExecuteNonQuery();
        }

        // Override the DatabaseManager's dbPath to use the in-memory database
        var dbManager = testObject.AddComponent<DatabaseManager>();
        Assert.IsNotNull(dbManager, "DatabaseManager component should not be null.");

        var connectionField = typeof(DatabaseManager).GetField("connection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.IsNotNull(connectionField, "DatabaseManager connection field should not be null.");
        connectionField.SetValue(dbManager, connection);

        login.databaseCall = dbManager;
        Assert.IsNotNull(login.databaseCall, "DatabaseManager should not be null.");
    }

    [TearDown]
    public void TearDown()
    {
        // Destroy the test GameObject if it exists
        if (testObject != null)
        {
            Object.DestroyImmediate(testObject);
        }

        // Close the database connection if it exists
        if (connection != null && connection.State == ConnectionState.Open)
        {
            connection.Close();
        }
    }

    [Test]
    public void OnLoginButtonClick_ShowsSuccessPopupForValidCredentials()
    {
        // Arrange
        login.usernameInputField.text = "TestUser";
        login.passwordInputField.text = "TestPassword";

        // Mock the success notification popup
        var successPopupObject = new GameObject("SuccessPopup");
        var successText = new GameObject("SuccessText").AddComponent<TextMeshProUGUI>();
        successPopupObject.AddComponent<Canvas>(); // Add a Canvas component to simulate UI behavior
        login.successCall.notificationPopup = successPopupObject;
        login.successCall.notificationText = successText;

        // Act
        login.OnLoginButtonClick();

        // Assert
        Assert.AreEqual("Login successful for: TestUser", successText.text, "Success popup should display the correct message.");
    }

    [Test]
    public void OnLoginButtonClick_ShowsErrorPopupForInvalidCredentials()
    {
        // Arrange
        login.usernameInputField.text = "TestUser";
        login.passwordInputField.text = "WrongPassword";

        // Mock the error notification popup
        var errorPopupObject = new GameObject("ErrorPopup");
        var errorText = new GameObject("ErrorText").AddComponent<TextMeshProUGUI>();
        errorPopupObject.AddComponent<Canvas>(); // Add a Canvas component to simulate UI behavior
        login.errorCall.notificationPopup = errorPopupObject;
        login.errorCall.notificationText = errorText;

        // Act
        login.OnLoginButtonClick();

        // Assert
        Assert.AreEqual("Username or password is incorrect.", errorText.text, "Error popup should display the correct message.");
    }

    [Test]
    public void OnLoginButtonClick_DoesNothingForEmptyUsernameOrPassword()
    {
        // Arrange
        login.usernameInputField.text = ""; // Empty username
        login.passwordInputField.text = "TestPassword"; // Valid password

        // Mock the success notification popup
        var successPopupObject = new GameObject("SuccessPopup");
        var successText = new GameObject("SuccessText").AddComponent<TextMeshProUGUI>();
        successPopupObject.AddComponent<Canvas>(); // Add a Canvas component to simulate UI behavior
        login.successCall.notificationPopup = successPopupObject;
        login.successCall.notificationText = successText;

        // Mock the error notification popup
        var errorPopupObject = new GameObject("ErrorPopup");
        var errorText = new GameObject("ErrorText").AddComponent<TextMeshProUGUI>();
        errorPopupObject.AddComponent<Canvas>(); // Add a Canvas component to simulate UI behavior
        login.errorCall.notificationPopup = errorPopupObject;
        login.errorCall.notificationText = errorText;

        // Act
        login.OnLoginButtonClick();

        // Assert
        Assert.IsFalse(successPopupObject.activeSelf, "Success popup should not be shown.");
        Assert.IsFalse(errorPopupObject.activeSelf, "Error popup should not be shown.");
        Assert.IsEmpty(successText.text, "Success popup text should be empty.");
        Assert.IsEmpty(errorText.text, "Error popup text should be empty.");
    }

    [Test]
    public void OnLoginButtonClick_SetsGlobalUserForValidCredentials()
    {
        // Arrange
        login.usernameInputField.text = "user1";
        login.passwordInputField.text = "123456";

        // Act
        login.OnLoginButtonClick();

        // Assert
        Assert.AreEqual("user1", Global.GlobalUser, "Global.GlobalUser should be set to the logged-in username.");
    }

    [UnityTest]
    public IEnumerator OnLoginButtonClick_DelaysSceneChangeForValidCredentials()
    {
        // Arrange
        login.usernameInputField.text = "user1";
        login.passwordInputField.text = "123456";
        login.delayBeforeSceneChange = 2f;

        // Mock the SceneLoader
        var mockSceneLoader = new MockSceneLoader();
        login.sceneCall = mockSceneLoader;

        // Act
        login.OnLoginButtonClick();

        // Wait for the delay
        yield return new WaitForSeconds(login.delayBeforeSceneChange + 0.1f);

        // Assert
        Assert.IsTrue(mockSceneLoader.MainSceneLoaded, "Main scene should be loaded after the delay.");
    }
}