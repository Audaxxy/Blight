using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MouseButton", menuName = "ScriptableObjects/Modules/MouseButton")]
public class ModuleButtonMouse : AbstractModuleButton
{
    public MouseButton mouseButton = MouseButton.None;

    protected override bool GetPressed()
    {
        return GetButtonPressed();
    }

    protected override bool GetTapped()
    {
        return GetButtonTapped();
    }

    bool GetButtonPressed()
    {
        switch (mouseButton)
        {
            case MouseButton.Left:
                return Input.GetMouseButton(0);
            case MouseButton.Right:
                return Input.GetMouseButton(1);
            case MouseButton.Middle:
                return Input.GetMouseButton(2);
            default:
                return false;
        }
    }

    bool GetButtonTapped()
    {
        switch (mouseButton)
        {
            case MouseButton.Left:
                return Input.GetMouseButtonDown(0);
            case MouseButton.Right:
                return Input.GetMouseButtonDown(1);
            case MouseButton.Middle:
                return Input.GetMouseButtonDown(2);
            default:
                return false;
        }
    }

    public enum MouseButton
    {
        None,
        Left,
        Right,
        Middle
    }
}
