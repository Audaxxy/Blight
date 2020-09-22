using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyboardButton", menuName = "ScriptableObjects/Modules/KeyboardButton")]
public class ModuleButtonKeyboard : AbstractModuleButton
{
    public KeyType button = KeyType.None;

    protected override bool GetPressed()
    {
        return Input.GetKey(Key(button));
    }

    protected override bool GetTapped()
    {
        return Input.GetKeyDown(Key(button));
    }
}
