using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;  // This is required for scene management

/// <summary>
/// Handles all scene change methods
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public bool MainSceneLoaded { get; private set; } = false;


    /// <summary>
    /// Load Main Screen when button is clicked
    /// </summary>
    public void LoadMainScene()
    {
        // The name of the scene you want to load (make sure it's added to Build Settings)
        SceneManager.LoadScene("MainScreen");
    }

    /// <summary>
    /// Loads Login Screen
    /// </summary>
    public void LoadLoginScene()
    {
        SceneManager.LoadScene("LoginScreen");
    }

    /// <summary>
    /// Loads Register User Screen
    /// </summary>
    public void LoadRegisterScene()
    {
        SceneManager.LoadScene("RegisterScreen");
    }

    /// <summary>
    /// Loads Stats Screen (deprecated)
    /// </summary>
    public void LoadStatScene()
    {
        //SceneManager.LoadScene("StatScreen");
    }

    /// <summary>
    /// Loads Demo Environment
    /// </summary>
    public void LoadDemoScene()
    {
        SceneManager.LoadScene("EnvTest");
    }

    /// <summary>
    /// Loads Environment 1
    /// </summary>
    public void LoadEnv1Scene()
    {
        SceneManager.LoadScene("EnvTest2");
    }

    /// <summary>
    /// Loads Report Screen
    /// </summary>
    public void LoadReportScene()
    {
        SceneManager.LoadScene("ReportScreen");
    }

    public void LoadScene(string sceneName)
    {
        if (Application.isPlaying)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            EditorSceneManager.OpenScene($"Assets/Scenes/{sceneName}.unity");
        }
    }
}
