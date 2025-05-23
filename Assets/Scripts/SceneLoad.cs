using UnityEngine;
using UnityEngine.SceneManagement;  // This is required for scene management

public class SceneLoader : MonoBehaviour
{
    // This method will be called when the button is clicked
    public void LoadMainScene()
    {
        // The name of the scene you want to load (make sure it's added to Build Settings)
        SceneManager.LoadScene("MainScreen");
    }

    public void LoadLoginScene()
    {
        SceneManager.LoadScene("LoginScreen");
    }

    public void LoadRegisterScene()
    {
        SceneManager.LoadScene("RegisterScreen");
    }

    public void LoadStatScene()
    {
        //SceneManager.LoadScene("StatScreen");
    }

    public void LoadDemoScene()
    {
        SceneManager.LoadScene("EnvTest");
    }

    public void LoadEnv1Scene()
    {
        SceneManager.LoadScene("EnvTest2");
    }

    public void LoadReportScene()
    {
        SceneManager.LoadScene("ReportScreen");
    }
}
