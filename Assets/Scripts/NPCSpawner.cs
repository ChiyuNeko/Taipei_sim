using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    private NPCsManager npcsManager;
    public List<GameObject> npcPrefabs = new List<GameObject>();

    [SerializeField] List<Transform> nowSpawnPoint;
    [SerializeField] List<Transform> nowDestinationPosition;

    public float spawnDelayTime = 0.2f;  // �ͦ�����ɶ�
    public int maxNPCCount;         // ����ͦ���NPC�ƶq

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
        GameObject NPCsParent = new GameObject("NPCs"); // �Ыؤ@�Ӥ�����Ӧs��ͦ���NPC

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

    //����ɩI�s
    public Transform GetRandomDestinations()
    {
        int rendomIndex = Random.Range(0, nowDestinationPosition.Count);
        return nowDestinationPosition[rendomIndex];
    }
}
