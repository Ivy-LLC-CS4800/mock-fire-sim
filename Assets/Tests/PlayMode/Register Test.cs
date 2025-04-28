using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class RegisterTests
{
    private GameObject testObject;
    private Register register;

    /// <summary>
    /// Sets up the test environment by creating a GameObject with the `Register` component
    /// and mocking its UI elements and dependencies.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        // Create a GameObject and attach the Register component
        testObject = new GameObject("RegisterTestObject");
        register = testObject.AddComponent<Register>();

        // Mock UI elements
        register.usernameInputField = new GameObject("UsernameInputField").AddComponent<InputField>();
        register.passwordInputField = new GameObject("PasswordInputField").AddComponent<InputField>();
        register.registerButton = new GameObject("RegisterButton").AddComponent<Button>();
        register.lengthConditionText = new GameObject("LengthConditionText").AddComponent<TextMeshProUGUI>();

        // Mock dependencies using custom mock classes
        register.sceneCall = new MockSceneLoader();
        register.databaseCall = new MockDatabaseManager();
        register.errorCall = new MockNotification();
        register.successCall = new MockNotification();
    }

    /// <summary>
    /// Cleans up the test environment by destroying the test GameObject and any associated objects.
    /// </summary>
    [TearDown]
    public void TearDown()
    {
        // Destroy the test GameObject and any associated objects
        if (testObject != null)
            Object.DestroyImmediate(testObject);

        // Destroy any UI elements or mock objects created
        if (register.usernameInputField != null)
            Object.DestroyImmediate(register.usernameInputField.gameObject);

        if (register.passwordInputField != null)
            Object.DestroyImmediate(register.passwordInputField.gameObject);

        if (register.registerButton != null)
            Object.DestroyImmediate(register.registerButton.gameObject);

        if (register.lengthConditionText != null)
            Object.DestroyImmediate(register.lengthConditionText.gameObject);
    }

    /// <summary>
    /// Test: Verify that the `OnRegisterButtonClick` method handles registration correctly.
    /// Predicted: The success or error notifications should be shown based on the input.
    /// Checked: The `successCall` or `errorCall` methods are invoked as expected.
    /// </summary>
    [UnityTest]
    public IEnumerator OnRegisterButtonClick_HandlesRegistrationCorrectly()
    {
        // Arrange
        var mockDatabase = (MockDatabaseManager)register.databaseCall;
        var mockSuccessNotification = (MockNotification)register.successCall;
        var mockErrorNotification = (MockNotification)register.errorCall;

        // Case 1: Successful registration
        register.usernameInputField.text = "NewUser";
        register.passwordInputField.text = "ValidPassword";
        mockDatabase.ShouldRegisterSuccessfully = true;

        // Act
        register.OnRegisterButtonClick();
        yield return null;

        // Assert
        Assert.IsTrue(mockSuccessNotification.WasPopupShown, "Success notification should be shown for successful registration.");
        Assert.AreEqual("User registered: NewUser", mockSuccessNotification.LastMessage);

        // Reset mock state
        mockSuccessNotification.Reset();
        mockErrorNotification.Reset();

        // Case 2: Username already exists
        register.usernameInputField.text = "ExistingUser";
        register.passwordInputField.text = "ValidPassword";
        mockDatabase.ShouldRegisterSuccessfully = false;

        // Act
        register.OnRegisterButtonClick();
        yield return null;

        // Assert
        Assert.IsTrue(mockErrorNotification.WasPopupShown, "Error notification should be shown for failed registration.");
        Assert.AreEqual("Username already exists.", mockErrorNotification.LastMessage);
    }
}

/// <summary>
/// Custom mock class for `SceneLoader`.
/// </summary>
public class MockSceneLoader : SceneLoader
{
    public string LastLoadedScene { get; private set; }

    public override void LoadScene(string sceneName)
    {
        LastLoadedScene = sceneName;
    }
}

/// <summary>
/// Custom mock class for `DatabaseManager`.
/// </summary>
public class MockDatabaseManager : DatabaseManager
{
    public bool ShouldRegisterSuccessfully { get; set; }

    public override bool RegisterUsername(string username, string password)
    {
        return ShouldRegisterSuccessfully;
    }
}

/// <summary>
/// Custom mock class for `Notification`.
/// </summary>
public class MockNotification : Notification
{
    public bool WasPopupShown { get; private set; }
    public string LastMessage { get; private set; }

    public override void ShowPopup(string message)
    {
        WasPopupShown = true;
        LastMessage = message;
    }

    public void Reset()
    {
        WasPopupShown = false;
        LastMessage = null;
    }
}