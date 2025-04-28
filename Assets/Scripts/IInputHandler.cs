using UnityEditor.IMGUI.Controls;
using UnityEngine;

public interface IInputHandler
{
    bool GetKey(KeyCode key);
    bool GetKeyDown(KeyCode key);
    float GetAxis(string axisName);
}