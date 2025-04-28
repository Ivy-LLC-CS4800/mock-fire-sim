using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using System.Collections;

public class PlayerTest
{
    private GameObject testObject;
    private Player player;
    private GameObject mockPickableObject;
    private GameObject mockUseableObject;

    /// <summary>
    /// Sets up the test environment by creating a GameObject with the `Player` component
    /// and initializing its required properties.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        // Create a GameObject and attach the Player component
        testObject = new GameObject("PlayerTestObject");
        player = testObject.AddComponent<Player>();

        // Mock required components and objects
        player.playerCameraTransform = new GameObject("PlayerCamera").transform;
        player.pickUpUI = new GameObject("PickUpUI");
        player.useableUI = new GameObject("UseableUI");
        player.pickUpParent = new GameObject("PickUpParent").transform;

        // Mock pickable object
        mockPickableObject = new GameObject("PickableObject");
        mockPickableObject.AddComponent<BoxCollider>();
        mockPickableObject.AddComponent<Rigidbody>();
        mockPickableObject.AddComponent<MockPickable>();

        // Mock useable object
        mockUseableObject = new GameObject("UseableObject");
        mockUseableObject.AddComponent<BoxCollider>();
        mockUseableObject.AddComponent<MockUseableFloor>();
    }

    [TearDown]
    public void TearDown()
    {
        // Destroy all created objects
        Object.DestroyImmediate(testObject);
        Object.DestroyImmediate(mockPickableObject);
        Object.DestroyImmediate(mockUseableObject);
    }

    /// <summary>
    /// Test: Verify that the `Start` method assigns input actions to their respective event handlers.
    /// Predicted: The input actions should have event handlers assigned.
    /// Checked: The input actions are verified to have non-null event handlers.
    /// </summary>
    [Test]
    public void Start_AssignsInputActions()
    {
        // Act
        player.Start();

        // Assert
        Assert.IsNotNull(player.interactionInput.action, "Interaction input action should be assigned.");
        Assert.IsNotNull(player.dropInput.action, "Drop input action should be assigned.");
        Assert.IsNotNull(player.useInput.action, "Use input action should be assigned.");
    }

    /// <summary>
    /// Test: Verify that the player can pick up an object when the raycast detects a valid `IPickable` object.
    /// Predicted: The object should be parented to the `pickUpParent`, and `inHandItem` should reference the object.
    /// Checked: The parent of the object and the value of `inHandItem` are compared to the expected values.
    /// </summary>
    [UnityTest]
    public IEnumerator PickUp_PicksUpObject()
    {
        // Arrange
        player.Start();
        player.inHandItem = null;
        // Simulate a RaycastHit by creating a new instance and assigning the collider
        var raycastHit = new RaycastHit
        {
            point = mockPickableObject.transform.position,
            normal = Vector3.up,
            distance = 1.0f
        };

        // Use reflection to set the private field `pickableHit` (if it's private)
        var pickableHitField = typeof(Player).GetField("pickableHit", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        pickableHitField.SetValue(player, raycastHit);

        // Act
        player.PickUp(new InputAction.CallbackContext());
        yield return null;

        // Assert
        Assert.AreEqual(mockPickableObject, player.inHandItem, "The picked-up object should be assigned to inHandItem.");
        Assert.AreEqual(player.pickUpParent, mockPickableObject.transform.parent, "The picked-up object should be parented to pickUpParent.");
    }

    /// <summary>
    /// Test: Verify that the player can drop an object.
    /// Predicted: The object should no longer be parented to the `pickUpParent`, and `inHandItem` should be null.
    /// Checked: The parent of the object and the value of `inHandItem` are compared to the expected values.
    /// </summary>
    [UnityTest]
    public IEnumerator Drop_DropsObject()
    {
        // Arrange
        player.Start();
        player.inHandItem = mockPickableObject;
        mockPickableObject.transform.SetParent(player.pickUpParent);

        // Act
        player.Drop(new InputAction.CallbackContext());
        yield return null;

        // Assert
        Assert.IsNull(player.inHandItem, "inHandItem should be null after dropping the object.");
        Assert.IsNull(mockPickableObject.transform.parent, "The dropped object should no longer have a parent.");
    }

    /// <summary>
    /// Test: Verify that the player can use an object when the raycast detects a valid `IUseableFloor` object.
    /// Predicted: The `Use` method of the `IUseableFloor` object should be called with the `inHandItem`.
    /// Checked: The `Use` method is verified to be called once with the expected parameters.
    /// </summary>
    [UnityTest]
    public IEnumerator Use_UsesObject()
    {
        // Arrange
        player.Start();
        player.inHandItem = mockPickableObject;
        // Simulate a RaycastHit by creating a new instance
        // Position the mockUseableObject in front of the player
        mockUseableObject.transform.position = player.transform.position + player.transform.forward * 2.0f;

        // Perform a raycast to populate the RaycastHit
        Ray ray = new Ray(player.transform.position, player.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 3.0f, Color.red, 1.0f);
        RaycastHit raycastHit;
        Assert.IsTrue(Physics.Raycast(ray, out raycastHit, 3.0f), "Raycast should hit the mockUseableObject.");

        // Use reflection to set the private field `useableHit` (if it's private)
        var useableHitField = typeof(Player).GetField("useableHit", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        useableHitField.SetValue(player, raycastHit);

        var mockUseable = mockUseableObject.GetComponent<MockUseableFloor>();
        mockUseable.ResetCallCount();

        // Act
        player.Use(new InputAction.CallbackContext());
        yield return null;

        // Assert
        Assert.AreEqual(1, mockUseable.CallCount, "The Use method should be called once.");
        Assert.AreEqual(mockPickableObject, mockUseable.LastUsedItem, "The Use method should be called with the inHandItem.");
    }
}