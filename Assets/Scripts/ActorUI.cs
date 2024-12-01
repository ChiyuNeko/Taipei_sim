using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActorUI : MonoBehaviour
{
    //public citizensMovement _cm;

    public Text debugText;
    public Text MyPosistionText;
    private void Start()
    {
        debugText.text = "";
    }

    public void DBugLog(string msg)
    {
        debugText.text += " >> " + msg;
    }

    public void ShowDestination(string msg)
    {
        MyPosistionText.text = msg;
    }
}
