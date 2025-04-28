// using System.Collections;
// using NUnit.Framework;
// using UnityEngine;
// using UnityEngine.TestTools;

// public class MoveControllerTest
// {
//     private GameObject testObject;
//     private MoveController moveController;
//     private Animator animator;

//     /// <summary>
//     /// Sets up the test environment by creating a GameObject with the `MoveController` component
//     /// and attaching a mock `Animator` component.
//     /// </summary>
//     [SetUp]
//     public void SetUp()
//     {
//         // Create a GameObject and attach the MoveController component
//         testObject = new GameObject("MoveControllerTestObject");
//         moveController = testObject.AddComponent<MoveController>();

//         // Attach a mock Animator component
//         animator = testObject.AddComponent<Animator>();
//     }

//     [TearDown]
//     public void TearDown()
//     {
//         // Destroy the test GameObject
//         Object.DestroyImmediate(testObject);
//     }

//     /// Test: Verify that the `Start` method initializes the animator and hashes correctly.
//     /// Predicted: The `Animator` component should be assigned, and all hash values should be initialized.
//     /// Checked: The `animator` reference and hash values are compared to their expected states.
//     [Test]
//     public void Start_InitializesAnimatorAndHashes()
//     {
//         // Act
//         moveController.Start();

//         // Assert
//         Assert.IsNotNull(moveController.GetType().GetField("animator", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(moveController), "Animator should be initialized.");
//         Assert.AreNotEqual(0, moveController.GetType().GetField("isWalkingHash", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(moveController), "isWalkingHash should be initialized.");
//         Assert.AreNotEqual(0, moveController.GetType().GetField("isRunningHash", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(moveController), "isRunningHash should be initialized.");
//     }

//     /// Test: Verify that pressing the "W" key sets the `isWalking` parameter to true.
//     /// Predicted: The `isWalking` parameter in the animator should be set to true when "W" is pressed.
//     /// Checked: The `isWalking` parameter in the animator is compared to `true`.
//     [UnityTest]
//     public IEnumerator Update_WalkingForward_SetsIsWalkingTrue()
//     {
//         // Arrange
//         moveController.Start();
//         animator.SetBool("isWalking", false);

//         // Simulate pressing the "W" key
//         Input.GetKey = (key) => key == "w";

//         // Act
//         moveController.Update();
//         yield return null;

//         // Assert
//         Assert.IsTrue(animator.GetBool("isWalking"), "isWalking should be true when 'W' is pressed.");
//     }

//     /// Test: Verify that releasing the "W" key sets the `isWalking` parameter to false.
//     /// Predicted: The `isWalking` parameter in the animator should be set to false when "W" is released.
//     /// Checked: The `isWalking` parameter in the animator is compared to `false`.
//     [UnityTest]
//     public IEnumerator Update_StoppingWalkingForward_SetsIsWalkingFalse()
//     {
//         // Arrange
//         moveController.Start();
//         animator.SetBool("isWalking", true);

//         // Simulate releasing the "W" key
//         Input.GetKey = (key) => false;

//         // Act
//         moveController.Update();
//         yield return null;

//         // Assert
//         Assert.IsFalse(animator.GetBool("isWalking"), "isWalking should be false when 'W' is released.");
//     }

//     /// Test: Verify that pressing "Left Shift" and "W" sets the `isRunning` parameter to true.
//     /// Predicted: The `isRunning` parameter in the animator should be set to true when "Left Shift" and "W" are pressed.
//     /// Checked: The `isRunning` parameter in the animator is compared to `true`.
//     [UnityTest]
//     public IEnumerator Update_RunningForward_SetsIsRunningTrue()
//     {
//         // Arrange
//         moveController.Start();
//         animator.SetBool("isRunning", false);

//         // Simulate pressing "Left Shift" and "W"
//         Input.GetKey = (key) => key == "w" || key == "left shift";

//         // Act
//         moveController.Update();
//         yield return null;

//         // Assert
//         Assert.IsTrue(animator.GetBool("isRunning"), "isRunning should be true when 'Left Shift' and 'W' are pressed.");
//     }

//     /// Test: Verify that releasing "Left Shift" or "W" sets the `isRunning` parameter to false.
//     /// Predicted: The `isRunning` parameter in the animator should be set to false when "Left Shift" or "W" is released.
//     /// Checked: The `isRunning` parameter in the animator is compared to `false`.
//     [UnityTest]
//     public IEnumerator Update_StoppingRunning_SetsIsRunningFalse()
//     {
//         // Arrange
//         moveController.Start();
//         animator.SetBool("isRunning", true);

//         // Simulate releasing "Left Shift" or "W"
//         Input.GetKey = (key) => false;

//         // Act
//         moveController.Update();
//         yield return null;

//         // Assert
//         Assert.IsFalse(animator.GetBool("isRunning"), "isRunning should be false when 'Left Shift' or 'W' is released.");
//     }
    
//     /// <summary>
//     /// Test: Verify that pressing the "A" key sets the `isWalkingLeft` parameter to true.
//     /// Predicted: The `isWalkingLeft` parameter in the animator should be set to true when "A" is pressed.
//     /// Checked: The `isWalkingLeft` parameter in the animator is compared to `true`.
//     /// </summary>
//     [UnityTest]
//     public IEnumerator Update_WalkingLeft_SetsIsWalkingLeftTrue()
//     {
//         // Arrange
//         moveController.Start();
//         animator.SetBool("isWalkingLeft", false);

//         // Simulate pressing the "A" key
//         Input.GetKey = (key) => key == "a";

//         // Act
//         moveController.Update();
//         yield return null;

//         // Assert
//         Assert.IsTrue(animator.GetBool("isWalkingLeft"), "isWalkingLeft should be true when 'A' is pressed.");
//     }

//     /// <summary>
//     /// Test: Verify that pressing the "D" key sets the `isWalkingRight` parameter to true.
//     /// Predicted: The `isWalkingRight` parameter in the animator should be set to true when "D" is pressed.
//     /// Checked: The `isWalkingRight` parameter in the animator is compared to `true`.
//     /// </summary>
//     [UnityTest]
//     public IEnumerator Update_WalkingRight_SetsIsWalkingRightTrue()
//     {
//         // Arrange
//         moveController.Start();
//         animator.SetBool("isWalkingRight", false);

//         // Simulate pressing the "D" key
//         Input.GetKey = (key) => key == "d";

//         // Act
//         moveController.Update();
//         yield return null;

//         // Assert
//         Assert.IsTrue(animator.GetBool("isWalkingRight"), "isWalkingRight should be true when 'D' is pressed.");
//     }

//     /// Test: Verify that pressing the "S" key sets the `isWalkingBackwards` parameter to true.
//     /// Predicted: The `isWalkingBackwards` parameter in the animator should be set to true when "S" is pressed.
//     /// Checked: The `isWalkingBackwards` parameter in the animator is compared to `true`.
//     [UnityTest]
//     public IEnumerator Update_WalkingBackwards_SetsIsWalkingBackwardsTrue()
//     {
//         // Arrange
//         moveController.Start();
//         animator.SetBool("isWalkingBackwards", false);

//         // Simulate pressing the "S" key
//         Input.GetKey = (key) => key == "s";

//         // Act
//         moveController.Update();
//         yield return null;

//         // Assert
//         Assert.IsTrue(animator.GetBool("isWalkingBackwards"), "isWalkingBackwards should be true when 'S' is pressed.");
//     }

//     /// Test: Verify that releasing all movement keys sets all movement-related parameters to false.
//     /// Predicted: All movement-related parameters in the animator should be set to false when no movement keys are pressed.
//     /// Checked: Each movement-related parameter in the animator is compared to `false`.
//     [UnityTest]
//     public IEnumerator Update_StoppingAllMovement_SetsAllParametersFalse()
//     {
//         // Arrange
//         moveController.Start();
//         animator.SetBool("isWalking", true);
//         animator.SetBool("isRunning", true);
//         animator.SetBool("isWalkingLeft", true);
//         animator.SetBool("isWalkingRight", true);
//         animator.SetBool("isWalkingBackwards", true);

//         // Simulate releasing all movement keys
//         Input.GetKey = (key) => false;

//         // Act
//         moveController.Update();
//         yield return null;

//         // Assert
//         Assert.IsFalse(animator.GetBool("isWalking"), "isWalking should be false when no keys are pressed.");
//         Assert.IsFalse(animator.GetBool("isRunning"), "isRunning should be false when no keys are pressed.");
//         Assert.IsFalse(animator.GetBool("isWalkingLeft"), "isWalkingLeft should be false when no keys are pressed.");
//         Assert.IsFalse(animator.GetBool("isWalkingRight"), "isWalkingRight should be false when no keys are pressed.");
//         Assert.IsFalse(animator.GetBool("isWalkingBackwards"), "isWalkingBackwards should be false when no keys are pressed.");
//     }
// }

// class MockInputHandler : IInputHandler
// {
//     private readonly System.Func<KeyCode, bool> getKeyFunc;
//     private readonly System.Func<KeyCode, bool> getKeyDownFunc;
//     private readonly System.Func<string, float> getAxisFunc;

//     public MockInputHandler(
//         System.Func<KeyCode, bool> getKeyFunc,
//         System.Func<KeyCode, bool> getKeyDownFunc,
//         System.Func<string, float> getAxisFunc)
//     {
//         this.getKeyFunc = getKeyFunc;
//         this.getKeyDownFunc = getKeyDownFunc;
//         this.getAxisFunc = getAxisFunc;
//     }

//     public bool GetKey(KeyCode key) => getKeyFunc(key);
//     public bool GetKeyDown(KeyCode key) => getKeyDownFunc(key);
//     public float GetAxis(string axisName) => getAxisFunc(axisName);
// }