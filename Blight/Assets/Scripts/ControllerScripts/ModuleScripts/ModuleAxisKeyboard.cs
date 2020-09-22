using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyboardAxis", menuName = "ScriptableObjects/Modules/KeyboardAxis")]
public class ModuleAxisKeyboard : AbstractModuleAxis
{
    public KeyType increase = KeyType.None;
    public KeyType decrease = KeyType.None;

    public override float GetAxis()
    {
        float output = 0f;

        if (Input.GetKey(Key(increase)))
            output += 1f;
        if (Input.GetKey(Key(decrease)))
            output -= 1f;

        return output;
    }
}
