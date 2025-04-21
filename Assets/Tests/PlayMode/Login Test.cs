using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using Moq; // Use Moq for mocking dependencies
using UnityEngine.TestTools;
using System.Collections;

public class LoginTest
{
    private GameObject testObject;
    private Login login;

    /// <summary>
    /// Sets up the test environment by creating a GameObject with the `Login` component
    /// and mocking its UI elements and dependencies.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        // Create a GameObject and attach the Login component
        testObject = new GameObject("LoginTestObject");
        login = testObject.AddComponent<Login>();

        // Mock UI elements
        login.usernameInputField = new GameObject("UsernameInputField").AddComponent<InputField>();
        login.passwordInputField = new GameObject("PasswordInputField").AddComponent<InputField>();
        login.loginButton = new GameObject("LoginButton").AddComponent<Button>();
        login.registerButton = new GameObject("RegisterButton").AddComponent<Button>();

        // Mock dependencies
        login.sceneCall = new Mock<SceneLoader>().Object;
        login.databaseCall = new Mock<DatabaseManager>().Object;
        login.errorCall = new Mock<Notification>().Object;
        login.successCall = new Mock<Notification>().Object();
    }

    [TearDown]
    public void TearDown()
    {
        // Destroy the test GameObject and any associated objects
        Object.DestroyImmediate(testObject);
    }

    /// <summary>
    /// Test: Verify that the `Start` method adds listeners to the login and register buttons.
    /// Predicted: The `onClick` listeners for both buttons should not be null after `Start` is called.
    /// Checked: The `onClick` listeners for `loginButton` and `registerButton` are compared to `null`.
    /// </summary>
    [Test]
    public void Start_AddsButtonListeners()
    {
        // Act
        login.Start();

        // Assert
        Assert.IsNotNull(login.loginButton.onClick, "Login button should have a listener attached.");
        Assert.IsNotNull(login.registerButton.onClick, "Register button should have a listener attached.");
    }

    /// <summary>
    /// Test: Verify that `OnLoginButtonClick` shows a success popup when the username and password are correct.
    /// Predicted: The `successCall.ShowPopup` method should be called with the correct message.
    /// Checked: The `successCall.ShowPopup` method is verified to be called once with the expected message.
    /// </summary>
    [Test]
    public void OnLoginButtonClick_ShowsSuccessPopupForValidCredentials()
    {
        // Arrange
        string testUsername = "TestUser";
        string testPassword = "TestPassword";
        login.usernameInputField.text = testUsername;
        login.passwordInputField.text = testPassword;

        var mockDatabase = Mock.Get(login.databaseCall);
        mockDatabase.Setup(db => db.CheckUsernameAndPassword(testUsername, testPassword)).Returns(true);

        var mockSuccessCall = Mock.Get(login.successCall);

        // Act
        login.OnLoginButtonClick();

        // Assert
        mockSuccessCall.Verify(sc => sc.ShowPopup($"Login successful for: {testUsername}"), Times.Once);
    }

    /// <summary>
    /// Test: Verify that `OnLoginButtonClick` shows an error popup when the username or password is incorrect.
    /// Predicted: The `errorCall.ShowPopup` method should be called with the correct error message.
    /// Checked: The `errorCall.ShowPopup` method is verified to be called once with the expected message.
    /// </summary>
    [Test]
    public void OnLoginButtonClick_ShowsErrorPopupForInvalidCredentials()
    {
        // Arrange
        string testUsername = "TestUser";
        string testPassword = "WrongPassword";
        login.usernameInputField.text = testUsername;
        login.passwordInputField.text = testPassword;

        var mockDatabase = Mock.Get(login.databaseCall);
        mockDatabase.Setup(db => db.CheckUsernameAndPassword(testUsername, testPassword)).Returns(false);

        var mockErrorCall = Mock.Get(login.errorCall);

        // Act
        login.OnLoginButtonClick();

        // Assert
        mockErrorCall.Verify(ec => ec.ShowPopup("Username or password is incorrect."), Times.Once);
    }

    /// <summary>
    /// Test: Verify that `OnRegisterButtonClick` calls the `LoadRegisterScene` method.
    /// Predicted: The `sceneCall.LoadRegisterScene` method should be called once.
    /// Checked: The `sceneCall.LoadRegisterScene` method is verified to be called once.
    /// </summary>
    [Test]
    public void OnRegisterButtonClick_CallsLoadRegisterScene()
    {
        // Arrange
        var mockSceneCall = Mock.Get(login.sceneCall);

        // Act
        login.OnRegisterButtonClick();

        // Assert
        mockSceneCall.Verify(sc => sc.LoadRegisterScene(), Times.Once);
    }

    /// <summary>
    /// Test: Verify that `ChangeToMainScene` calls the `LoadMainScene` method.
    /// Predicted: The `sceneCall.LoadMainScene` method should be called once.
    /// Checked: The `sceneCall.LoadMainScene` method is verified to be called once.
    /// </summary>
    [Test]
    public void ChangeToMainScene_CallsLoadMainScene()
    {
        // Arrange
        var mockSceneCall = Mock.Get(login.sceneCall);

        // Act
        login.GetType().GetMethod("ChangeToMainScene", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(login, null);

        // Assert
        mockSceneCall.Verify(sc => sc.LoadMainScene(), Times.Once);
    }

    /// <summary>
    /// Test: Verify that `OnLoginButtonClick` does nothing when the username or password is empty.
    /// Predicted: Neither the success nor error popup should be shown, and no database calls should be made.
    /// Checked: Ensure that `successCall.ShowPopup`, `errorCall.ShowPopup`, and `databaseCall.CheckUsernameAndPassword` are not called.
    /// </summary>
    [Test]
    public void OnLoginButtonClick_DoesNothingForEmptyUsernameOrPassword()
    {
        // Arrange
        login.usernameInputField.text = "";
        login.passwordInputField.text = "TestPassword";

        var mockDatabase = Mock.Get(login.databaseCall);
        var mockSuccessCall = Mock.Get(login.successCall);
        var mockErrorCall = Mock.Get(login.errorCall);

        // Act
        login.OnLoginButtonClick();

        // Assert
        mockDatabase.Verify(db => db.CheckUsernameAndPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        mockSuccessCall.Verify(sc => sc.ShowPopup(It.IsAny<string>()), Times.Never);
        mockErrorCall.Verify(ec => ec.ShowPopup(It.IsAny<string>()), Times.Never);
    }

    /// <summary>
    /// Test: Verify that `Global.GlobalUser` is set correctly after a successful login.
    /// Predicted: The `Global.GlobalUser` variable should be set to the username of the logged-in user.
    /// Checked: The value of `Global.GlobalUser` is compared to the username.
    /// </summary>
    [Test]
    public void OnLoginButtonClick_SetsGlobalUserForValidCredentials()
    {
        // Arrange
        string testUsername = "TestUser";
        string testPassword = "TestPassword";
        login.usernameInputField.text = testUsername;
        login.passwordInputField.text = testPassword;

        var mockDatabase = Mock.Get(login.databaseCall);
        mockDatabase.Setup(db => db.CheckUsernameAndPassword(testUsername, testPassword)).Returns(true);

        // Act
        login.OnLoginButtonClick();

        // Assert
        Assert.AreEqual(testUsername, Global.GlobalUser, "Global.GlobalUser should be set to the logged-in username.");
    }

    /// <summary>
    /// Test: Verify that the scene transition is delayed by the specified amount of time after a successful login.
    /// Predicted: The `LoadMainScene` method should be called after the delay specified by `delayBeforeSceneChange`.
    /// Checked: Use a coroutine to wait for the delay and verify the method call.
    /// </summary>
    [UnityTest]
    public IEnumerator OnLoginButtonClick_DelaysSceneChangeForValidCredentials()
    {
        // Arrange
        string testUsername = "TestUser";
        string testPassword = "TestPassword";
        login.usernameInputField.text = testUsername;
        login.passwordInputField.text = testPassword;
        login.delayBeforeSceneChange = 2f;

        var mockDatabase = Mock.Get(login.databaseCall);
        mockDatabase.Setup(db => db.CheckUsernameAndPassword(testUsername, testPassword)).Returns(true);

        var mockSceneCall = Mock.Get(login.sceneCall);

        // Act
        login.OnLoginButtonClick();

        // Wait for the delay
        yield return new WaitForSeconds(login.delayBeforeSceneChange + 0.1f);

        // Assert
        mockSceneCall.Verify(sc => sc.LoadMainScene(), Times.Once);
    }
}