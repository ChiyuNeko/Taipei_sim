using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterManager : MonoBehaviour
{
    public DisasterDropper iceBallDropper;
    public DisasterDropper meteoriteDropper;
    public DisasterDropper RainDropper;

    public void DropDisasterByName(Vector3 posistion, string name)
    {
        Vector3 DropPosistion = posistion;
        switch(name)
        {
            case "HugIceBall":
                iceBallDropper.OnDisaster(DropPosistion);
                break;
            case "meteorite":
                meteoriteDropper.OnDisaster(DropPosistion);
                break;
            case "Rain":
                RainDropper.OnDisaster(DropPosistion);
                break;

            default:
                Debug.LogError($"No disaster name \"{name}\"!");
                break;
        }
    }

}
