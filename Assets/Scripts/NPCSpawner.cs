using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    private NPCsManager npcsManager;
    public List<GameObject> npcPrefabs = new List<GameObject>();

    [SerializeField] List<Transform> nowSpawnPoint;
    [SerializeField] List<Transform> nowDestinationPosition;

    public float spawnDelayTime = 0.2f;  // 生成延遲時間
    public int maxNPCCount;         // 控制生成的NPC數量

    private void Awake()
    {
        npcsManager = GameObject.FindGameObjectWithTag("GameSceneManager").GetComponent<NPCsManager>();
    }

    public void SpawnNPC(int scal, Transform SpawnPoint, Transform DestinationPosition)
    {
        maxNPCCount = scal;

        nowSpawnPoint.Clear();
        nowDestinationPosition.Clear();

        GetAllDestinations(DestinationPosition);
        GetAllSpawnPosistion(SpawnPoint);

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

            Transform randomSpawnPoint = nowSpawnPoint[Random.Range(0, nowSpawnPoint.Count)];

            GameObject newNPC = Instantiate(npcPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation, NPCsParent.transform);
            NPC_Movement nPC_Movement = newNPC.GetComponent<NPC_Movement>();
            nPC_Movement.NPCsManager = npcsManager;
            nPC_Movement.destinations = nowDestinationPosition;

            newNPC.SetActive(true);
            npcsManager.spawnNpcs.Add(newNPC);
            currentNPCCount++;
            yield return new WaitForSeconds(spawnDelayTime);
        }

    }

    private void GetAllDestinations(Transform ParentObject)
    {
        foreach (Transform child in ParentObject)
        {
            nowDestinationPosition.Add(child);
        }
    }
    private void GetAllSpawnPosistion(Transform ParentObject)
    {
        foreach (Transform spawnPoint in ParentObject)
        {
            nowSpawnPoint.Add(spawnPoint);
        }
    }

    //執行時呼叫
    public Transform GetRandomDestinations()
    {
        int rendomIndex = Random.Range(0, nowDestinationPosition.Count);
        return nowDestinationPosition[rendomIndex];
    }
}
