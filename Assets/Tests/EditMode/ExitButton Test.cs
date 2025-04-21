using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ExitButtonScriptTests
{
    private GameObject exitButtonGO;
    private ExitButtonScript exitButton;

    [SetUp]
    public void Setup()
    {
        // Create a new GameObject and attach the ExitButtonScript
        exitButtonGO = new GameObject("ExitButton");
        exitButton = exitButtonGO.AddComponent<ExitButtonScript>();
    }

    [TearDown]
    public void Teardown()
    {
        if (Application.isPlaying)
            Object.Destroy(exitButtonGO);
        else
            Object.DestroyImmediate(exitButtonGO);
    }

    [UnityTest]
    public IEnumerator ExitGame_LogsExitMessage()
    {
        LogAssert.Expect(LogType.Log, "Exit button pressed");

        // Act
        exitButton.ExitGame();

        // Wait a frame for the log to appear
        yield return null;
    }
}
