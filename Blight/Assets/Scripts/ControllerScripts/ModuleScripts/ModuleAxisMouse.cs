using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MouseAxis", menuName = "ScriptableObjects/Modules/MouseAxis")]
public class ModuleAxisMouse : AbstractModuleAxis
{
    public float scrollAmplifier = 1f;
    public float mouseSensitivity = 1f;

    public Axis mouseAxis = Axis.None;

    public override float GetAxis()
    {
        return mouseDirection(mouseAxis);
    }

    float mouseDirection(Axis axis)
    {
        switch (axis)
        {
            case Axis.MouseX:
                return Input.GetAxis("Mouse X") * mouseSensitivity;
            case Axis.MouseInvertX:
                return -Input.GetAxis("Mouse X") * mouseSensitivity;
            case Axis.MouseY:
                return Input.GetAxis("Mouse Y") * mouseSensitivity;
            case Axis.MouseInvertY:
                return -Input.GetAxis("Mouse Y") * mouseSensitivity;
            case Axis.MouseScroll:
                return Input.mouseScrollDelta.y * scrollAmplifier;
            case Axis.MouseInvertScroll:
                return -Input.mouseScrollDelta.y * scrollAmplifier;
            default:
                return 0;
        }
    }

    public enum Axis
    {
        None,
        MouseX,
        MouseInvertX,
        MouseY,
        MouseInvertY,
        MouseScroll,
        MouseInvertScroll
    }
}
