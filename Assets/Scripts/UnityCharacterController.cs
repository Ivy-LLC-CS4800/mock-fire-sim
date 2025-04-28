using UnityEngine;

public class UnityCharacterController : ICharacterController
{
    private readonly CharacterController characterController;

    public UnityCharacterController(CharacterController characterController)
    {
        this.characterController = characterController;
    }

    public bool IsGrounded => characterController.isGrounded;
}