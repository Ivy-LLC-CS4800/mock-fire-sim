using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.TestTools;
using System.Collections;

public class NotificationTest
{
    private GameObject testObject;
    private Notification notification;

    /// <summary>
    /// Sets up the test environment by creating a GameObject with the `Notification` component
    /// and mocking its UI elements (popup and text).
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        // Create a GameObject and attach the Notification component
        testObject = new GameObject("NotificationTestObject");
        notification = testObject.AddComponent<Notification>();

        // Mock UI elements
        notification.notificationPopup = new GameObject("NotificationPopup");
        notification.notificationText = notification.notificationPopup.AddComponent<TextMeshProUGUI>();

        // Ensure the popup starts inactive
        notification.notificationPopup.SetActive(false);
    }

    [TearDown]
    public void TearDown()
    {
        // Destroy the test GameObject and any associated objects
        Object.DestroyImmediate(testObject);
    }

    /// <summary>
    /// Test: Verify that the notification popup is hidden by default.
    /// Predicted: The `notificationPopup` GameObject should be inactive when the scene starts.
    /// Checked: The `notificationPopup.activeSelf` property is compared to `false`.
    /// </summary>
    [Test]
    public void Start_HidesNotificationPopupByDefault()
    {
        // Act
        notification.Start();

        // Assert
        Assert.IsFalse(notification.notificationPopup.activeSelf, "Notification popup should be hidden by default.");
    }

    /// <summary>
    /// Test: Verify that `ShowPopup` displays the notification popup with the correct message.
    /// Predicted: The `notificationPopup` should become active, and the `notificationText` should display the provided message.
    /// Checked: The `notificationPopup.activeSelf` property and `notificationText.text` are compared to the expected values.
    /// </summary>
    [Test]
    public void ShowPopup_DisplaysNotificationWithCorrectMessage()
    {
        // Arrange
        string testMessage = "Test Notification";

        // Act
        notification.ShowPopup(testMessage);

        // Assert
        Assert.IsTrue(notification.notificationPopup.activeSelf, "Notification popup should be active after calling ShowPopup.");
        Assert.AreEqual(testMessage, notification.notificationText.text, "Notification text should match the provided message.");
    }

    /// <summary>
    /// Test: Verify that `HidePopup` hides the notification popup.
    /// Predicted: The `notificationPopup` GameObject should become inactive after calling `HidePopup`.
    /// Checked: The `notificationPopup.activeSelf` property is compared to `false`.
    /// </summary>
    [Test]
    public void HidePopup_HidesNotificationPopup()
    {
        // Arrange
        notification.notificationPopup.SetActive(true); // Ensure the popup is active

        // Act
        notification.HidePopup();

        // Assert
        Assert.IsFalse(notification.notificationPopup.activeSelf, "Notification popup should be hidden after calling HidePopup.");
    }

    /// <summary>
    /// Test: Verify that `ShowPopup` automatically hides the notification popup after 2 seconds.
    /// Predicted: The `notificationPopup` should become inactive 2 seconds after calling `ShowPopup`.
    /// Checked: The `notificationPopup.activeSelf` property is compared to `false` after a delay.
    /// </summary>
    [UnityTest]
    public IEnumerator ShowPopup_HidesNotificationAfterDelay()
    {
        // Arrange
        string testMessage = "Test Notification";

        // Act
        notification.ShowPopup(testMessage);

        // Assert that the popup is initially active
        Assert.IsTrue(notification.notificationPopup.activeSelf, "Notification popup should be active immediately after calling ShowPopup.");

        // Wait for 2 seconds
        yield return new WaitForSeconds(2.1f);

        // Assert that the popup is now inactive
        Assert.IsFalse(notification.notificationPopup.activeSelf, "Notification popup should be hidden after 2 seconds.");
    }
}