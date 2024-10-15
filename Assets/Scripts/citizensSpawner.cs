using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public List<GameObject> npcPrefabs;  // 儲存所有的NPC prefab
    public Transform spawnPointParent;   // NPC生成位置的父物件 (spawnPoint_citizens)
    public float spawnDelayTime = 0.2f;    // 生成延遲時間
    public int maxNPCCount = 10;         // 控制生成的NPC數量

    private List<Transform> spawnPoints = new List<Transform>();  // 儲存所有子物件的生成點
    private int currentNPCCount = 0;



    void Start()
    {
        // 初始化所有生成點
        foreach (Transform spawnPoint in spawnPointParent)
        {
            spawnPoints.Add(spawnPoint);
        }

        // 開始自動生成NPC
        StartCoroutine(SpawnNPCs());
    }

    IEnumerator SpawnNPCs()
    {
        while (currentNPCCount < maxNPCCount)
        {
            // 隨機選擇一個NPC prefab
            GameObject npcPrefab = npcPrefabs[Random.Range(0, npcPrefabs.Count)];

            // 隨機選擇一個生成點
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            // 生成NPC
            Instantiate(npcPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);

            // 增加生成的NPC數量
            currentNPCCount++;

            // 等待下一次生成
            yield return new WaitForSeconds(spawnDelayTime);
        }
    }
}
