using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This will become beautiful later
public class NetworkManager
{
    //Master boolean designed to notify if this game is online
    public bool connectedToHost { get; private set; } = false;

    //constructor, empty currently
    private NetworkManager()
    {

    }

    //Instance
    private static NetworkManager instance;
    public static NetworkManager Instance {
        get { if (instance == null) { instance = new NetworkManager(); } return instance; }
    }

    //ID relative to the server, not used currently
    public int ReservedID { get; private set; } = -1;

    #region EventSendingAndReception
    //Used to send events to other clients. If in single player, the event just get immedaitely received.
    public void SendEvent(BaseEntityEvent bee)
    {
        if (!connectedToHost)
        {
            DeliverEvent(bee);
        }

        //Do other stuff if connected
    }

    //Used to take raw bytes and create a packet. Logic will be determined later.
    public void ReconstructEvent(byte[] bytes)
    {
        BaseEntityEvent bee = null;

        //Change later
        EVENT_TYPE type = EVENT_TYPE.MESSAGE;

        switch(type)
        {
        case EVENT_TYPE.MESSAGE:
                break;
        }

        if (bee != null)
        {
            DeliverEvent(bee);
        }
    }

    //Invoke event upon reception
    public void DeliverEvent(BaseEntityEvent bee)
    {
        bee.InvokeEvent();
    }
    #endregion

    //Functions designed to get variables from bytes.
    //Will be used to make events from packets.
    //Each function contains a reference to the array position of the byte it's currently reading from, and the list of bytes
    //Array position is automatically incremented in the function to avoid excess coding
    //Returns a variable as requested
    #region extract data
    public int PullInt(ref int iter, ref byte[] bits)
    {
        int toReturn = System.BitConverter.ToInt32(bits, iter);
        iter += sizeof(int);

        return toReturn;
    }

    public float PullFloat(ref int iter, ref byte[] bits)
    {
        float toReturn = System.BitConverter.ToSingle(bits, iter);
        iter += sizeof(float);

        return toReturn;
    }

    public char PullChar(ref int iter, ref byte[] bits)
    {
        char toReturn = System.BitConverter.ToChar(bits, iter);
        iter += sizeof(char);

        return toReturn;
    }

    public string PullString(ref int iter, ref byte[] bits)
    {
        string toReturn = "";
        int stringSize = PullInt(ref iter, ref bits);

        for (int i = 0; i < stringSize; ++i)
        {
            PullChar(ref iter, ref bits);
        }

        return toReturn;
    }
    #endregion

    //The opposite of the functions above, used to disintigrate variables into bytes
    //Takes in an array position, a ref to a byte array, and a piece of data
    //In the future the network manager will have designated byte arrays for sending and receiving data
    #region insert data
    public void PackInt(ref int iter, ref byte[] bits, int toPack)
    {
        System.BitConverter.GetBytes(toPack).CopyTo(bits, iter);
        iter += sizeof(int);
    }

    public void PackFloat(ref int iter, ref byte[] bits, float toPack)
    {
        System.BitConverter.GetBytes(toPack).CopyTo(bits, iter);
        iter += sizeof(float);
    }

    public void PackChar(ref int iter, ref byte[] bits, char toPack)
    {
        System.BitConverter.GetBytes(toPack).CopyTo(bits, iter);
        iter += sizeof(char);
    }

    public void PackString(ref int iter, ref byte[] bits, string toPack)
    {
        PackInt(ref iter, ref bits, toPack.Length);

        for (int i = 0; i < toPack.Length; ++i)
        {
            PackChar(ref iter, ref bits, toPack[i]);
        }
    }
    #endregion
}
