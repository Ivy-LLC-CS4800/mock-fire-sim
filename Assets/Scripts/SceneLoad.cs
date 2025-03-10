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

    public void LoadStatScene()
    {
        //SceneManager.LoadScene("StatScreen");
    }

    public void LoadDemoScene()
    {
        SceneManager.LoadScene("EnvTest");
    }

    public void LoadReportScene()
    {
        SceneManager.LoadScene("RScreenAlt");
    }

    public void LoadDBScene()
    {
        SceneManager.LoadScene("ReportScreen");
    }
}
