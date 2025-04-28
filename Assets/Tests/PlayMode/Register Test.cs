using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Moq; // Use Moq or a similar library for mocking if needed.

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

        // Mock dependencies
        register.sceneCall = new Mock<SceneLoader>().Object;
        register.databaseCall = new Mock<DatabaseManager>().Object;
        register.errorCall = new Mock<Notification>().Object;
        register.successCall = new Mock<Notification>().Object();
    }

    [TearDown]
    public void TearDown()
    {
        // Destroy the test GameObject and any associated objects
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

    /// Test: Verify that the password validation updates the condition text correctly.
    /// Predicted: For a valid password, the text color should be green. For an invalid password, the text color should be red.
    /// Checked: The `lengthConditionText.text` and `lengthConditionText.color` properties are compared to the expected values.
    [Test]
    public void ValidatePassword_UpdatesConditionTextCorrectly()
    {
        // Arrange
        string validPassword = "123456";
        string invalidPassword = "123";

        // Act
        register.ValidatePassword(validPassword);

        // Assert for valid password
        Assert.AreEqual("At least 6 characters", register.lengthConditionText.text);
        Assert.AreEqual(register.validColor, register.lengthConditionText.color);

        // Act
        register.ValidatePassword(invalidPassword);

        // Assert for invalid password
        Assert.AreEqual("At least 6 characters", register.lengthConditionText.text);
        Assert.AreEqual(register.invalidColor, register.lengthConditionText.color);
    }




}

    
   