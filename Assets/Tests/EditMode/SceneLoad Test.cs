using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTest
{
    private SceneLoader sceneLoader;
    private string loadedSceneName;

    /// <summary>
    /// Sets up the test environment by creating a GameObject with the `SceneLoader` component
    /// and mocking the `SceneManager.LoadScene` method to capture the scene name.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        // Create a GameObject and attach the SceneLoader component
        var gameObject = new GameObject("SceneLoader");
        sceneLoader = gameObject.AddComponent<SceneLoader>();

        // Mock SceneManager.LoadScene by capturing the scene name
        loadedSceneName = null; // Reset the loaded scene name
        var originalLoadSceneMethod = typeof(SceneManager).GetMethod("LoadScene", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(string) }, null);
        Assert.IsNotNull(originalLoadSceneMethod, "SceneManager.LoadScene method not found.");

        // Replace the method with a mock
        // SceneManager.LoadScene = (sceneName) => { loadedSceneName = sceneName; };
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up the created GameObject
        Object.DestroyImmediate(sceneLoader.gameObject);
    }

    /// Test: Verify that `LoadLoginScene` calls `SceneManager.LoadScene` with the correct scene name.
    /// Predicted: The `SceneManager.LoadScene` method should be called with "LoginScreen".
    /// Checked: The `loadedSceneName` variable is compared to "LoginScreen".
    [Test]
    public void LoadLoginScene_CallsSceneManagerWithCorrectSceneName()
    {
        // Act
        sceneLoader.LoadLoginScene();

        // Assert
        Assert.AreEqual("LoginScreen", loadedSceneName, "LoadLoginScene did not call SceneManager.LoadScene with the correct scene name.");
    }

    /// Test: Verify that `LoadRegisterScene` calls `SceneManager.LoadScene` with the correct scene name.
    /// Predicted: The `SceneManager.LoadScene` method should be called with "RegisterScreen".
    /// Checked: The `loadedSceneName` variable is compared to "RegisterScreen".
    [Test]
    public void LoadRegisterScene_CallsSceneManagerWithCorrectSceneName()
    {
        // Act
        sceneLoader.LoadRegisterScene();

        // Assert
        Assert.AreEqual("RegisterScreen", loadedSceneName, "LoadRegisterScene did not call SceneManager.LoadScene with the correct scene name.");
    }

    /// Test: Verify that `LoadDemoScene` calls `SceneManager.LoadScene` with the correct scene name.
    /// Predicted: The `SceneManager.LoadScene` method should be called with "EnvTest".
    /// Checked: The `loadedSceneName` variable is compared to "EnvTest".
    [Test]
    public void LoadDemoScene_CallsSceneManagerWithCorrectSceneName()
    {
        // Act
        sceneLoader.LoadDemoScene();

        // Assert
        Assert.AreEqual("EnvTest", loadedSceneName, "LoadDemoScene did not call SceneManager.LoadScene with the correct scene name.");
    }  

    /// Test: Verify that `LoadEnv1Scene` calls `SceneManager.LoadScene` with the correct scene name.
    /// Predicted: The `SceneManager.LoadScene` method should be called with "EnvTest2".
    /// Checked: The `loadedSceneName` variable is compared to "EnvTest2".
    [Test]
    public void LoadEnv1Scene_CallsSceneManagerWithCorrectSceneName()
    {
        // Act
        sceneLoader.LoadEnv1Scene();

        // Assert
        Assert.AreEqual("EnvTest2", loadedSceneName, "LoadEnv1Scene did not call SceneManager.LoadScene with the correct scene name.");
    }

    /// Test: Verify that `LoadReportScene` calls `SceneManager.LoadScene` with the correct scene name.
    /// Predicted: The `SceneManager.LoadScene` method should be called with "ReportScreen".
    /// Checked: The `loadedSceneName` variable is compared to "ReportScreen".
    [Test]
    public void LoadReportScene_CallsSceneManagerWithCorrectSceneName()
    {
        // Act
        sceneLoader.LoadReportScene();

        // Assert
        Assert.AreEqual("ReportScreen", loadedSceneName, "LoadReportScene did not call SceneManager.LoadScene with the correct scene name.");
    }
}