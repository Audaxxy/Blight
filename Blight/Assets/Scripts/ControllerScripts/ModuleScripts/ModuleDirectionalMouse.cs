using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MouseDirectional", menuName = "ScriptableObjects/Modules/MouseDirectional")]
public class ModuleDirectionalMouse : AbstractModuleDirectional
{
    public float scrollAmplifier = 1f;
    public float mouseSensitivity = 1f;

    public Axis Xaxis = Axis.MouseDeltaX;
    public Axis Yaxis = Axis.MouseDeltaY;
    public Axis Zaxis = Axis.None;

    public override Vector3 GetDirectionalInput()
    {
        Vector3 output = Vector3.zero;

        output.x += mouseDirection(Xaxis);
        output.y += mouseDirection(Yaxis);
        output.z += mouseDirection(Zaxis);

        return output;
    }

    float mouseDirection(Axis axis)
    {
        switch (axis)
        {
            case Axis.MouseDeltaX:
                return Input.GetAxis("Mouse X") * mouseSensitivity;
            case Axis.MouseDeltaInvertX:
                return -Input.GetAxis("Mouse X") * mouseSensitivity;
            case Axis.MouseDeltaY:
                return Input.GetAxis("Mouse Y") * mouseSensitivity;
            case Axis.MouseDeltaInvertY:
                return -Input.GetAxis("Mouse Y") * mouseSensitivity;
            case Axis.MouseScroll:
                return Input.mouseScrollDelta.y * scrollAmplifier;
            case Axis.MouseInvertScroll:
                return -Input.mouseScrollDelta.y * scrollAmplifier;
            case Axis.MousePositionX:
                return Input.mousePosition.x;
            case Axis.MousePositionInvertX:
                return -Input.mousePosition.x;
            case Axis.MousePositionY:
                return Input.mousePosition.y;
            case Axis.MousePositionInvertY:
                return -Input.mousePosition.y;
            default:
                return 0;
        }
    }

    public enum Axis
    {
        None,
        MouseDeltaX,
        MouseDeltaInvertX,
        MouseDeltaY,
        MouseDeltaInvertY,
        MouseScroll,
        MouseInvertScroll,
        MousePositionX,
        MousePositionInvertX,
        MousePositionY,
        MousePositionInvertY,
    }
}
