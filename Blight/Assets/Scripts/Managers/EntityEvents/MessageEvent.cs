using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Just as an example
public class MessageEvent : BaseEntityEvent
{
    //We have the actual data the event must carry
    public string message;

    //We have one constructor which takes in a matching data type, and creates a byte array
    public MessageEvent(string msg) : base(EVENT_TYPE.MESSAGE)
    {
        message = msg;

        bitArray = new byte[msg.Length * sizeof(char) + sizeof(int)];

        int iter = 0;
        NetworkManager.Instance.PackString(ref iter, ref bitArray, msg);
    }

    //The other constructor does the opposite, taking in bytes and making useable data
    public MessageEvent(byte[] bits) : base(bits, EVENT_TYPE.MESSAGE)
    {
        bitArray = bits;

        int iter = 0;
        message = NetworkManager.Instance.PullString(ref iter, ref bits);
    }

    //However the event is meant to be processed
    public override void InvokeEvent()
    {
        Debug.Log(message);
    }
}
