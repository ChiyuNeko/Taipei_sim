using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public Taipei nowPosistion;

    public DisasterManager disasterManager;
    public NPCsManager npcsManager;

    public GameObject TaipeiStationMapObj;
    public GameObject Taipei101MapObject;

    private void Start()//開發測試用
    {
        Initialize();
        SetMap(Taipei.TaipeiStation);
    }

    public void Initialize()
    {
        TaipeiStationMapObj.SetActive(false);
        Taipei101MapObject.SetActive(false);
    }

    public void SetMap(Taipei taipei)
    {
        switch (taipei)
        {
            case Taipei.Taipei101:
                TaipeiStationMapObj.SetActive (false);
                Taipei101MapObject.SetActive (true);
                disasterManager.SetDropCenter(taipei);
                npcsManager.SetNpc(taipei,20);
                break;
            case Taipei.TaipeiStation:
                TaipeiStationMapObj.SetActive(false);
                Taipei101MapObject.SetActive(true);
                disasterManager.SetDropCenter(taipei);
                npcsManager.SetNpc(taipei, 20);
                break;
            default:
                Debug.LogError($"No map name \"{taipei}\"");
                break;
        }
    }
}

public enum Taipei
{
    Taipei101,
    TaipeiStation
}