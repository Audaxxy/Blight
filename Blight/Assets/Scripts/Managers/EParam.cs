using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void RawMethod<T>(T oldVal, T newVal, SubParam<T> param);

public abstract class EParam<T>
{
    public bool locked = false;

    [SerializeField]
    protected T Value;

    private SubParam<T> param;
    protected SubParam<T> Param
    {
        get
        {
            if (param == null)
                param = new SubParam<T>();
            return param;
        }
    }

    public T value
    {
        get { return Value; }
        set
        {
            if (locked)
                return;
            Value = value;
            InnerSet();
        }
    }

    public void OnValidate()
    {
        if (locked)
        {
            Value = Param.value;
            return;
        }
        DoValidate();
    }

    protected abstract void DoValidate();

    public abstract void ForceValidate();

    public void HardSet(T val)
    {
        Param.value = val;
        Value = val;
    }

    protected abstract void InnerSet();
}

public abstract class UEParam<T> : EParam<T>
{
    [SerializeField]
    protected UnityEvent<T, T, SubParam<T>> onValueChangedUnityEvent;
    public RawMethod<T> onValueChangedDelegate;

    protected override void InnerSet()
    {
        T oldVal = Param.value;
        Param.value = Value;
        onValueChangedUnityEvent?.Invoke(oldVal, Value, Param);
        onValueChangedDelegate?.Invoke(oldVal, Value, Param);
        Value = Param.value;
    }
}

[System.Serializable]
public class UEClass<T> : UEParam<T> where T : class
{
    protected override void DoValidate()
    {
        if (Value != Param.value)
        {
            InnerSet();
        }
    }

    public override void ForceValidate()
    {
        InnerSet();
    }
}

[System.Serializable]
public class UEStruct<T> : UEParam<T> where T : struct
{
    protected override void DoValidate()
    {
        if (!Value.Equals(Param.value))
        {
            InnerSet();
        }
    }

    public override void ForceValidate()
    {
        InnerSet();
    }
}

[System.Serializable]
public class UEEnum<T> : UEParam<T> where T : System.Enum
{
    [SerializeField]
    protected List<UEEnumType<T>> onValueChangedToSpecificUnityEvent;
    public List<DEnumType<T>> onValueChangedToSpecificDelegate { get; private set; }

    protected override void DoValidate()
    {
        if (!Value.Equals(Param.value))
        {
            InnerSet();
        }
    }

    public override void ForceValidate()
    {
        InnerSet();
    }

    protected override void InnerSet()
    {
        T oldVal = Param.value;
        Param.value = Value;
        onValueChangedUnityEvent?.Invoke(oldVal, Value, Param);

        foreach(UEEnumType<T> uee in onValueChangedToSpecificUnityEvent)
        {
            if (uee.stateForEventsToRun.Equals(Value))
            {
                uee.onValueChanged?.Invoke(oldVal, Value, Param);
                break;
            }
        }

        foreach (DEnumType<T> de in onValueChangedToSpecificDelegate)
        {
            if (de.stateForEventsToRun.Equals(Value))
            {
                de.onValueChanged?.Invoke(oldVal, Value, Param);
                break;
            }
        }

        Value = Param.value;
    }
}

[System.Serializable]
public class SubParam<T>
{
    [HideInInspector]
    public T value;
}

[System.Serializable]
public struct UEEnumType<T> where T : System.Enum
{
    public T stateForEventsToRun;
    public UnityEvent<T, T, SubParam<T>> onValueChanged;
}

[System.Serializable]
public struct DEnumType<T> where T : System.Enum
{
    public T stateForEventsToRun;
    public RawMethod<T> onValueChanged;
}