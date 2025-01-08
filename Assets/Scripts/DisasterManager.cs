using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterManager : MonoBehaviour
{
    public GameSceneManager _gameSceneManager;

    public DisasterDropper iceBallDropper;
    public DisasterDropper meteoriteDropper;
    public DisasterDropper RainDropper;

    public Transform dropCenterTaipeiStation;
    public Transform dropCenterTaipei101;

    private Transform dropCenter;

    public void DropDisasterByName(Vector3 posistion, string name)
    {
        Vector3 DropPosistion = dropCenter.position;
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

    public void SetDropCenter(Taipei taipeiLandMark)
    {
        switch(taipeiLandMark)
        {
            case Taipei.Taipei101:
                dropCenter = dropCenterTaipei101;
                break;
            case Taipei.TaipeiStation:
                dropCenter = dropCenterTaipeiStation;
                break;
            default:
                Debug.LogError($"No map name \"{taipeiLandMark}\"");
                break;
        }
    }
}
