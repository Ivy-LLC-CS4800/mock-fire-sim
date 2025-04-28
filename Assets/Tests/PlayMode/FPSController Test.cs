// using NUnit.Framework;
// using UnityEngine;
// using UnityEngine.TestTools;
// using System.Collections;

// public class FPSControllerTest
// {
//     private GameObject testObject;
//     private FPSController fpsController;
//     private CharacterController characterController;
//     private Camera playerCamera;

//     /// <summary>
//     /// Sets up the test environment by creating a GameObject with the `FPSController` component
//     /// and initializing its required components.
//     /// </summary>
//     [SetUp]
//     public void SetUp()
//     {
//         // Create a GameObject and attach the FPSController component
//         testObject = new GameObject("FPSControllerTestObject");
//         fpsController = testObject.AddComponent<FPSController>();

//         // Add required components
//         characterController = testObject.AddComponent<CharacterController>();
//         playerCamera = new GameObject("PlayerCamera").AddComponent<Camera>();
//         fpsController.playerCamera = playerCamera;
//     }

//     [TearDown]
//     public void TearDown()
//     {
//         // Destroy the test GameObject and any associated objects
//         Object.DestroyImmediate(testObject);
//     }

//     /// <summary>
//     /// Test: Verify that the `Start` method initializes the required components and locks the cursor.
//     /// Predicted: The `CharacterController` and `Animator` components should be initialized, and the cursor should be locked and invisible.
//     /// Checked: The `characterController`, `animator`, and cursor state are compared to their expected values.
//     /// </summary>
//     [Test]
//     public void Start_InitializesComponentsAndLocksCursor()
//     {
//         // Act
//         fpsController.Start();

//         // Assert
//         Assert.IsNotNull(fpsController.GetComponent<CharacterController>(), "CharacterController should be initialized.");
//         Assert.IsNotNull(fpsController.GetComponent<Animator>(), "Animator should be initialized.");
//         Assert.AreEqual(CursorLockMode.Locked, Cursor.lockState, "Cursor should be locked.");
//         Assert.IsFalse(Cursor.visible, "Cursor should be invisible.");
//     }

//     /// <summary>
//     /// Test: Verify that pressing the "W" key moves the player forward.
//     /// Predicted: The player's forward movement should match the walk speed.
//     /// Checked: The `moveDirection` vector is compared to the expected forward direction.
//     /// </summary>
//     [UnityTest]
//     public IEnumerator Update_MovesPlayerForwardWhenWPressed()
//     {
//         // Arrange
//         fpsController.Start();
//         Input.GetKey = (key) => key == KeyCode.W;

//         // Act
//         fpsController.Update();
//         yield return null;

//         // Assert
//         Vector3 expectedDirection = fpsController.transform.forward * fpsController.walkSpeed;
//         Assert.AreEqual(expectedDirection.x, fpsController.moveDirection.x, 0.1f, "Player should move forward when 'W' is pressed.");
//     }

//     /// <summary>
//     /// Test: Verify that pressing "Space" while grounded makes the player jump.
//     /// Predicted: The player's vertical velocity should match the jump power.
//     /// Checked: The `moveDirection.y` value is compared to the jump power.
//     /// </summary>
//     [UnityTest]
//     public IEnumerator Update_PlayerJumpsWhenSpacePressed()
//     {
//         // Arrange
//         fpsController.Start();
//         characterController.isGrounded = true;
//         Input.GetKeyDown = (key) => key == KeyCode.Space;

//         // Act
//         fpsController.Update();
//         yield return null;

//         // Assert
//         Assert.AreEqual(fpsController.jumpPower, fpsController.moveDirection.y, 0.1f, "Player should jump when 'Space' is pressed.");
//     }

//     /// <summary>
//     /// Test: Verify that gravity is applied when the player is in the air.
//     /// Predicted: The player's vertical velocity should decrease over time due to gravity.
//     /// Checked: The `moveDirection.y` value is compared before and after applying gravity.
//     /// </summary>
//     [UnityTest]
//     public IEnumerator Update_AppliesGravityWhenInAir()
//     {
//         // Arrange
//         fpsController.Start();

//         // Use reflection to set the internal grounded state
//         var isGroundedField = typeof(CharacterController).GetField("m_IsGrounded", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
//         isGroundedField.SetValue(characterController, true);

//         fpsController.InputHandler = new MockInputHandler(
//             key => false,
//             key => key == KeyCode.Space,
//             axis => 0f
//         );

//         // Act
//         fpsController.Update();
//         yield return null;

//         // Assert
//         Assert.Less(fpsController.moveDirection.y, 0, "Gravity should decrease the player's vertical velocity when in the air.");
//     }

//     /// <summary>
//     /// Test: Verify that mouse input rotates the camera and clamps the vertical rotation.
//     /// Predicted: The camera's vertical rotation should be clamped within the `lookXLimit`.
//     /// Checked: The camera's local rotation is compared to the expected clamped value.
//     /// </summary>
//     [UnityTest]
//     public IEnumerator Update_RotatesCameraAndClampsVerticalRotation()
//     {
//         // Arrange
//         fpsController.Start();
//         Input.GetAxis = (axis) => axis == "Mouse Y" ? 1f : 0f;

//         // Act
//         fpsController.Update();
//         yield return null;

//         // Assert
//         float clampedRotationX = Mathf.Clamp(fpsController.rotationX, -fpsController.lookXLimit, fpsController.lookXLimit);
//         Assert.AreEqual(clampedRotationX, fpsController.rotationX, 0.1f, "Camera's vertical rotation should be clamped within the lookXLimit.");
//     }

//     /// <summary>
//     /// Test: Verify that holding the "Left Shift" key increases the player's movement speed.
//     /// Predicted: The player's forward movement speed should match the `runSpeed` when "Left Shift" is pressed.
//     /// Checked: The `moveDirection` vector is compared to the expected forward direction with `runSpeed`.
//     /// </summary>
//     [UnityTest]
//     public IEnumerator Update_RunningIncreasesMovementSpeed()
//     {
//         // Arrange
//         fpsController.Start();
//         Input.GetKey = (key) => key == KeyCode.W || key == KeyCode.LeftShift;

//         // Act
//         fpsController.Update();
//         yield return null;

//         // Assert
//         Vector3 expectedDirection = fpsController.transform.forward * fpsController.runSpeed;
//         Assert.AreEqual(expectedDirection.x, fpsController.moveDirection.x, 0.1f, "Player should move faster when 'Left Shift' is pressed.");
//     }

//     /// <summary>
//     /// Test: Verify that releasing all movement keys stops the player's movement.
//     /// Predicted: The `moveDirection` vector should be zero when no movement keys are pressed.
//     /// Checked: The `moveDirection` vector is compared to `Vector3.zero`.
//     /// </summary>
//     [UnityTest]
//     public IEnumerator Update_StopsMovementWhenNoKeysPressed()
//     {
//         // Arrange
//         fpsController.Start();
//         Input.GetKey = (key) => false; // Simulate no keys being pressed

//         // Act
//         fpsController.Update();
//         yield return null;

//         // Assert
//         Assert.AreEqual(Vector3.zero, fpsController.moveDirection, "Player should stop moving when no keys are pressed.");
//     }

//     /// <summary>
//     /// Test: Verify that setting `canMove` to `false` prevents the player from moving.
//     /// Predicted: The `moveDirection` vector should remain zero when `canMove` is `false`.
//     /// Checked: The `moveDirection` vector is compared to `Vector3.zero`.
//     /// </summary>
//     [UnityTest]
//     public IEnumerator Update_DisablesMovementWhenCanMoveIsFalse()
//     {
//         // Arrange
//         fpsController.Start();
//         fpsController.canMove = false;
//         Input.GetKey = (key) => key == KeyCode.W;

//         // Act
//         fpsController.Update();
//         yield return null;

//         // Assert
//         Assert.AreEqual(Vector3.zero, fpsController.moveDirection, "Player should not move when canMove is false.");
//     }
// }