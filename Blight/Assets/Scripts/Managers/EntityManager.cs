using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager
{
    static EntityManager em;
    public static EntityManager Instance
    {
        get { if (em == null) { em = new EntityManager(); } return em; }
    }

    List<ActiveSlot> allActive;
    Queue<ActiveSlot> inactiveQueue;

    IHaveAnID requestingAccess = null;

    public bool CheckRequest(IHaveAnID be)
    {
        return be == requestingAccess;
    }

    private EntityManager()
    {
        allActive = new List<ActiveSlot>();
        inactiveQueue = new Queue<ActiveSlot>();
    }

    public T ForceCreateEntity<T>(T template, int tempID, long uniqueID) where T : MonoBehaviour, IHaveAnID
    {
        ActiveSlot newActive = null;

        while (tempID >= allActive.Count)
        {
            newActive = new ActiveSlot();
            allActive.Add(newActive);
            newActive.listID = allActive.Count - 1;
        }

        newActive = allActive[tempID];

        if (Application.isEditor)
        {
            if (newActive.IDowner != null)
            {
                Debug.LogError("ERROR: TRYING TO FORCE ID " + tempID + " WHEN " + allActive[tempID].IDowner.name + " ALREADY EXISTS THERE!");
                return null;
            }
        }

        AssignIDs(template, tempID, uniqueID);

        DenySelfAssignment(template);
        T newEntity = GetEntity(template);
        PermitSelfAssignment(template);

        RevokeIDs(template);

        newActive.IDowner = newEntity;
        newEntity.SetSlot(newActive);

        SetWritten(newEntity, true);
        return newEntity;
    }

    T GetEntity<T>(T template) where T : MonoBehaviour, IHaveAnID
    {
        return Object.Instantiate(template);
    }

    public void RegisterID(IHaveAnID needsID)
    {
        ActiveSlot newActive = null;

        if (Application.isEditor)
        {
            if (needsID == null)
            {
                Debug.LogError("ERROR: TRYING TO REGISTER ID OF NULL ITEM!");
                return;
            }
        }

        if (inactiveQueue.Count == 0)
        {
            newActive = new ActiveSlot();
            allActive.Add(newActive);
            newActive.listID = allActive.Count - 1;
        }
        else
        {
            newActive = inactiveQueue.Dequeue();

            if (Application.isEditor)
            {
                if (newActive.IDowner != null)
                {
                    Debug.LogError("ERROR: ENTITY ID " + newActive.listID + " IS ALREADY IN USE BY " + allActive[newActive.listID].IDowner.name);
                    return;
                }
            }
        }

        newActive.IDowner = needsID;

        AssignIDs(needsID, newActive.listID, -1);

        SetWritten(needsID, true);
    }



    public void UnregisterID(IHaveAnID toReturn)
    {
        if (Application.isEditor)
        {
            if (toReturn == null)
            {
                Debug.LogError("ERROR: RETURNING NULL ENTITY!");

                return;
            }

            if (toReturn.NetworkingID < 0)
            {
                Debug.LogError("ERROR: " + toReturn.name + " NEVER HAD ITS ID SET");

                return;
            }

            if (allActive[toReturn.NetworkingID].IDowner != toReturn)
            {
                Debug.LogError("ERROR: MISMATCH OF REGISTERED IDS FOR " + toReturn.name);
            }
        }

        int returningID = toReturn.NetworkingID;
        inactiveQueue.Enqueue(allActive[returningID]);

        allActive[returningID].IDowner = null;

        RevokeIDs(toReturn);
        SetWritten(toReturn, false);
    }

    void AssignIDs(IHaveAnID requester, int tempID, long uniqueID)
    {
        requestingAccess = requester;
        requester.QueueIDs(tempID, uniqueID);
        requestingAccess = null;
    }

    void RevokeIDs(IHaveAnID requester)
    {
        requestingAccess = requester;
        requester.AllowSelfAssignment(false);
        requestingAccess = null;
    }

    void DenySelfAssignment(IHaveAnID requester)
    {
        requestingAccess = requester;
        requester.AllowSelfAssignment(false);
        requestingAccess = null;
    }

    void PermitSelfAssignment(IHaveAnID requester)
    {
        requestingAccess = requester;
        requester.AllowSelfAssignment(true);
        requestingAccess = null;
    }

    void SetWritten(IHaveAnID requester, bool isWritten)
    {
        requestingAccess = requester;
        requester.SetWritten(isWritten);
        requestingAccess = null;
    }
}

public class ActiveSlot
{
    public int listID;
    public IHaveAnID IDowner;

    public ActiveSlot()
    {
        listID = -1;
        IDowner = null;
    }
}

public interface IHaveAnID
{
    bool Written { get; }
    long IndividualID { get; }
    int NetworkingID { get; }
    string name { get; }
    ActiveSlot slot { get; }

    void SetSlot(ActiveSlot aSlot);
    void QueueIDs(int networkID, long uniqueID);
    void ResetIDs();

    void AllowSelfAssignment(bool allow);

    void SetWritten(bool written);
}