using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The base event from which all should derive
//Nothing has been done so far regarding these
//Any action that has a dramatic impact on the game shoul have a corresponding event packet
//e.g. something dies
//And should be sent to the networking manager (even if in single player)
//Whereupon the manager decides how to deal with it.
//All major events should pass through the networking manager first before they're registered ingame
//This will help A TON when we include networking later on
public abstract class BaseEntityEvent
{
    //The type. An enum is used because it translates nicely into integer and bytes for networking.
    public EVENT_TYPE type { get; protected set; }

    //an array containing all the data from the event
    public byte[] bitArray;

    //Two constructors all derivatives of BaseEntityEvent must have
    //Regardless of which constructor is used, the actual data of the event and the byte array are created at once
    //This was made to help take a load off the network manager, although I do not know if this is better
    protected BaseEntityEvent(byte[] deconstructedEvent, EVENT_TYPE eType)
    {
        type = eType;
    }

    protected BaseEntityEvent(EVENT_TYPE eType)
    {
        type = eType;
    }

    //What is used to have the event do something
    public abstract void InvokeEvent();
}

//Expand as necessary
public enum EVENT_TYPE
{
    MESSAGE,
    PICKUP_WEAPON,
    PICKUP_SPELL,
    PICKUP_ACCESSORY,
    ATTACK,
    DEATH
}