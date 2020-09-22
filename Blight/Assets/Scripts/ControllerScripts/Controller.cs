using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Controller", menuName = "ScriptableObjects/Controller")]
public class Controller : ScriptableObject
{
    public AbstractModuleDirectional movement;
    public AbstractModuleButton lightAttack;
    public AbstractModuleButton dash;
}
