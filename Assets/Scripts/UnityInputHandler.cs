using UnityEngine;

public class UnityInputHandler : IInputHandler
{
    public bool GetKey(KeyCode key)
    {
        return Input.GetKey(key);
    }

    public bool GetKeyDown(KeyCode key)
    {
        return Input.GetKeyDown(key);
    }

    public float GetAxis(string axisName)
    {
        return Input.GetAxis(axisName);
    }
}