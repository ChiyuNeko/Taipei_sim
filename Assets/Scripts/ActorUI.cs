using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActorUI : MonoBehaviour
{
    public Text pathPending;

    public Text Destination;
    private void Start()
    {
        
    }

    public void DBugLog(bool isPathPending)
    {
        pathPending.text = "isPathPending >> " + isPathPending;
    }

    public void ShowDestination(string msg)
    {
        Destination.text = msg;
    }
}
