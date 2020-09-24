using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour, IHaveAnID
{
    [SerializeField]
    private bool isWritten = false;
    [SerializeField]
    private long individualID = -1;
    [SerializeField]
    private int networkingID = -1;
    private ActiveSlot mySlot;

    public bool Written => isWritten;

    public int NetworkingID => networkingID;

    public long IndividualID => individualID;

    public ActiveSlot slot => mySlot;

    private bool allowSelfAssignment = true;

    protected virtual void Awake()
    {
        SelfInitialize();
    }

    protected virtual void OnEnable()
    {
        SelfInitialize();
    }

    void SelfInitialize()
    {
        if (allowSelfAssignment && !Written)
        {
            EntityManager.Instance.RegisterID(this);
        }
    }

    protected virtual void OnDisable()
    {

    }

    protected virtual void OnDestroy()
    {
        SelfDestruct();
    }

    void SelfDestruct()
    {
        if (Written)
        {
            EntityManager.Instance.UnregisterID(this);
        }
    }

    public void SetSlot(ActiveSlot aSlot)
    {
        mySlot = aSlot;
    }

    public void QueueIDs(int networkID, long uniqueID)
    {
        if (EntityManager.Instance.CheckRequest(this))
        {
            networkingID = networkID;
            individualID = uniqueID;
        }
    }

    public void ResetIDs()
    {
        if (EntityManager.Instance.CheckRequest(this))
        {
            networkingID = -1;
            individualID = -1;
        }
    }

    public void AllowSelfAssignment(bool allow)
    {
        if (EntityManager.Instance.CheckRequest(this))
        {
            allowSelfAssignment = allow;
        }
    }

    public void SetWritten(bool written)
    {
        if (EntityManager.Instance.CheckRequest(this))
        {
            isWritten = written;
        }
    }
}
