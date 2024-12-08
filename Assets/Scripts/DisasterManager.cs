using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterManager : MonoBehaviour
{
    public hugIceBallDropper iceBallDropper;

    public Transform dropPosistion;


    private void Start()
    {
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            iceBallDropper.OnDisaster_hugIceBall(dropPosistion);
        }
    }

}
