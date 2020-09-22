using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractModuleButton : AbstractModule
{
    public bool tapped
    {
        get { return GetTapped(); }
    }

    public bool pressed
    {
        get { return GetPressed(); }
    }
    protected abstract bool GetPressed();

    protected abstract bool GetTapped();
}
