using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCsManager : MonoBehaviour
{
    [SerializeField] NPCSpawner spawner;

    public List<GameObject> spawnNpcs = new List<GameObject>();

    [Header("Taiper101")]
    public Transform spawnPointParent_Taipei101;
    public Transform destinationPositionParent_Taipei101;
    public Transform onAccidentPositionParent_Taipei101;

    [Header("TaiperStation")]
    public Transform spawnPointParent_TaipeiStation;
    public Transform destinationPositionParent_TaipeiStation;
    public Transform onAccidentPositionParent_TaipeiStation;

    [SerializeField] List<Transform> nowOnAccidentPosition;

    public void SetNpc(Taipei LandMark ,int num)
    {
        ClearAllSpawnNpc();
        nowOnAccidentPosition.Clear();

        switch (LandMark)
        {
            case Taipei.Taipei101:
                GetAllOnAccidentPosition(onAccidentPositionParent_Taipei101);
                spawner.SpawnNPC(num, spawnPointParent_Taipei101, destinationPositionParent_Taipei101);
                break;
            case Taipei.TaipeiStation:
                GetAllOnAccidentPosition(onAccidentPositionParent_TaipeiStation);
                spawner.SpawnNPC(num, spawnPointParent_TaipeiStation, destinationPositionParent_TaipeiStation);
                break;
        }
    }


    public Transform GetClosestSanctuaryPosition(Transform NPC)
    {
        // 檢查清單是否為空
        if (nowOnAccidentPosition == null || nowOnAccidentPosition.Count == 0)
        {
            Debug.LogWarning("No sanctuary positions available.");
            return null; // 返回空，避免錯誤
        }

        List<Transform> SanctuaryPosition = new List<Transform>(nowOnAccidentPosition);

        // 按與NPC的距離排序
        SanctuaryPosition.Sort((a, b) =>
            Vector3.Distance(NPC.position, a.position).CompareTo(
                Vector3.Distance(NPC.position, b.position)
            )
        );

        return SanctuaryPosition[0];
    }


    private void GetAllOnAccidentPosition(Transform ParentObject)
    {
        foreach (Transform onAccidentPosition in ParentObject)
        {
            nowOnAccidentPosition.Add(onAccidentPosition);
        }
    }

    public Transform GetRandomDestinations()
    {
        return spawner.GetRandomDestinations();
    }

    private void ClearAllSpawnNpc()
    {
        for (int i = 0; i< spawnNpcs.Count; i++)
        {
            Destroy(spawnNpcs[i]);
        }
        spawnNpcs.Clear();
    }
}
