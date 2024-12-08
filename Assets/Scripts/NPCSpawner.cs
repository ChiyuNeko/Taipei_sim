using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    NPCsManager npcsManager;
    public List<GameObject> npcPrefabs;  // 儲存所有的NPC prefab

    public Transform spawnPointParent;   // NPC生成位置的父物件 (spawnPoint_citizens)
    public List<Transform> spawnPoints;
    public Transform targetPositionParent;  // 包含所有目的地的父物件
    public List<Transform> targetPositions;
    public Transform onAccidentPositionParent;
    public List <Transform> onAccidentPositions;

    public float spawnDelayTime = 0.2f;  // 生成延遲時間
    public int maxNPCCount;         // 控制生成的NPC數量

    private void Start()
    {
        npcsManager = GetComponent<NPCsManager>();

        GetAllDestinations();
        GetAllSpawnPosistion();
        GetAllOnAccidentPosition();
    }

    public void SpawnNPC(int scal)
    {
        maxNPCCount = scal;
        StartCoroutine(SpawnNPCs());
    }

    IEnumerator SpawnNPCs()
    {
        int currentNPCCount = 0;

        Debug.Log("Start Spawn NPCs");
        GameObject NPCsParent = new GameObject("NPCs"); // 創建一個父物件來存放生成的NPC

        while (currentNPCCount < maxNPCCount)
        {
            GameObject npcPrefab = npcPrefabs[Random.Range(0, npcPrefabs.Count)];

            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            GameObject newNPC = Instantiate(npcPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation, NPCsParent.transform);
            NPC_Movement nPC_Movement = newNPC.GetComponent<NPC_Movement>();
            nPC_Movement.NPCsManager = npcsManager;
            nPC_Movement.destinations = targetPositions;


            newNPC.SetActive(true);
            npcsManager.NPCs.Add(newNPC);
            currentNPCCount++;
            yield return new WaitForSeconds(spawnDelayTime);
        }

    }

    void GetAllDestinations()
    {
        // 收集所有目的地
        foreach (Transform child in targetPositionParent)
        {
            targetPositions.Add(child);
        }
    }
    void GetAllSpawnPosistion()
    {
        foreach (Transform spawnPoint in spawnPointParent)
        {
            spawnPoints.Add(spawnPoint);
        }
    }
    void GetAllOnAccidentPosition()
    {
        foreach (Transform onAccidentPosition in onAccidentPositionParent)
        {
            onAccidentPositions.Add(onAccidentPosition); // 正確填充清單
        }
    }



    public Transform GetClosestSanctuaryPosition(Transform NPC)
    {
        // 檢查清單是否為空
        if (onAccidentPositions == null || onAccidentPositions.Count == 0)
        {
            Debug.LogWarning("No sanctuary positions available.");
            return null; // 返回空，避免錯誤
        }

        List<Transform> SanctuaryPosition = new List<Transform>(onAccidentPositions);

        // 按與NPC的距離排序
        SanctuaryPosition.Sort((a, b) =>
            Vector3.Distance(NPC.position, a.position).CompareTo(
                Vector3.Distance(NPC.position, b.position)
            )
        );

        return SanctuaryPosition[0];
    }

    public Transform GetRandomDestinations()
    {
        int rendomIndex = Random.Range(0, targetPositions.Count);
        return targetPositions[rendomIndex];
    }
}
