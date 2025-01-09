using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public Taipei nowPosistion;

    public DisasterManager disasterManager;
    public NPCsManager npcsManager;
    public GameObject Player;

    public GameObject TaipeiStationMapObj;
    public GameObject Taipei101MapObject;

    private void Start()//�}�o���ե�
    {
        Initialize();
        //SetMap(Taipei.Taipei101);
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
                npcsManager.SetNpc(taipei,50);
                break;
            case Taipei.TaipeiStation:
                TaipeiStationMapObj.SetActive(true);
                Taipei101MapObject.SetActive(false);
                disasterManager.SetDropCenter(taipei);
                npcsManager.SetNpc(taipei, 50);
                break;
            default:
                Debug.LogError($"No map name \"{taipei}\"");
                break;
        }
    }

    public void SetTaipei101(GameObject RespawnPoint)
    {
        SetMap(Taipei.Taipei101);
        Player.transform.position = RespawnPoint.transform.position;

    }

    public void SetTaipeiStation(GameObject RespawnPoint)
    {
        SetMap(Taipei.TaipeiStation);
        Player.transform.position = RespawnPoint.transform.position;
    }
}

public enum Taipei
{
    Taipei101,
    TaipeiStation
}