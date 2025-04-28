using NUnit.Framework;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Moq; // Use Moq or a similar library for mocking if needed.

public class ReportTests
{
    private GameObject testObject;
    private Report report;

    [SetUp]
    public void SetUp()
    {
        // Create a GameObject and attach the Report component
        testObject = new GameObject("ReportTestObject");
        report = testObject.AddComponent<Report>();

        // Mock UI elements
        report.titleText = new GameObject("TitleText").AddComponent<TextMeshProUGUI>();
        report.scoreText = new GameObject("ScoreText").AddComponent<TextMeshProUGUI>();

        // Mock sliders and labels
        report.taskSliders = new Slider[2];
        report.taskLabels = new TextMeshProUGUI[2];
        for (int i = 0; i < 2; i++)
        {
            report.taskSliders[i] = new GameObject($"TaskSlider_{i}").AddComponent<Slider>();
            report.taskLabels[i] = new GameObject($"TaskLabel_{i}").AddComponent<TextMeshProUGUI>();
        }
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up after each test
        Object.DestroyImmediate(testObject);
    }

    /// Test: Verify that the title text is set correctly based on the global user.
    /// Predicted: The title text should display "Report for 'TestUser'" when the global user is "TestUser".
    /// Checked: The `titleText.text` property is compared to the expected string.
    [Test]
    public void DisplayTitle_SetsCorrectTitle()
    {
        // Arrange
        Global.GlobalUser = "TestUser";

        // Act
        report.DisplayTitle();

        // Assert
        Assert.AreEqual("Report for 'TestUser'", report.titleText.text);
    }

    /// Test: Verify that the total score is displayed correctly for a given user.
    /// Predicted: The score text should display "Total Score for TestUser: 100%" based on the mock database.
    /// Checked: The `scoreText.text` property is compared to the expected string.
    [Test]
    public void GetTotalScore_DisplaysCorrectScore()
    {
        // Arrange
        string mockUserName = "TestUser";
        Global.GlobalUser = mockUserName;

        // Mock the database connection and query
        // You can use a mocking library like Moq or simulate the database behavior here.

        // Act
        report.GetType().GetMethod("GetTotalScore", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(report, new object[] { mockUserName });

        // Assert
        // Replace with the expected score based on your mock database
        Assert.AreEqual($"Total Score for {mockUserName}: 100%", report.scoreText.text);
    }

    /// Test: Verify that task completion updates the UI elements correctly.
    /// Predicted: Task labels and sliders should reflect the mock task data (e.g., "Task 1: 75%", "Task 2: 50%").
    /// Checked: The `taskLabels` text and `taskSliders` values are compared to the expected values.
    [Test]
    public void DisplayTaskCompletion_UpdatesUIElements()
    {
        // Arrange
        var mockTaskData = new Dictionary<string, int>
        {
            { "Task 1", 75 },
            { "Task 2", 50 }
        };

        // Mock the DisplayTaskHelper method to return mock data
        var method = report.GetType().GetMethod("DisplayTaskHelper", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method.Invoke(report, null);

        // Act
        report.DisplayTaskCompletion();

        // Assert
        Assert.AreEqual("Task 1: 75%", report.taskLabels[0].text);
        Assert.AreEqual("Task 2: 50%", report.taskLabels[1].text);
    }

    /// Test: Verify that subtask completion percentage is calculated correctly.
    /// Predicted: For 3 completed subtasks and 1 error subtask out of 4, the completion percentage should be 70%.
    /// Checked: The result of `GetSubtaskCompletion` is compared to the expected percentage.
    [Test]
    public void GetSubtaskCompletion_CalculatesCorrectPercentage()
    {
        // Arrange
        int mockTaskId = 1;
        int mockSubtaskCount = 4;

        // Mock the database query for subtasks
        // Simulate the database returning 3 completed subtasks and 1 error subtask.

        // Act
        var result = (float)report.GetType().GetMethod("GetSubtaskCompletion", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(report, new object[] { mockTaskId, mockSubtaskCount });

        // Assert
        Assert.AreEqual(70f, result); // Example: 3 completed, 1 error -> 70% completion
    }
}