using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    NPCsManager npcsManager;
    public List<GameObject> npcPrefabs;  // �x�s�Ҧ���NPC prefab

    public Transform spawnPointParent;   // NPC�ͦ���m�������� (spawnPoint_citizens)
    public List<Transform> spawnPoints;
    public Transform targetPositionParent;  // �]�t�Ҧ��ت��a��������
    public List<Transform> targetPositions;
    public Transform onAccidentPositionParent;
    public List <Transform> onAccidentPositions;

    public float spawnDelayTime = 0.2f;  // �ͦ�����ɶ�
    public int maxNPCCount;         // ����ͦ���NPC�ƶq

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
        GameObject NPCsParent = new GameObject("NPCs"); // �Ыؤ@�Ӥ�����Ӧs��ͦ���NPC

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
        // �����Ҧ��ت��a
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
            onAccidentPositions.Add(onAccidentPosition); // ���T��R�M��
        }
    }



    public Transform GetClosestSanctuaryPosition(Transform NPC)
    {
        // �ˬd�M��O�_����
        if (onAccidentPositions == null || onAccidentPositions.Count == 0)
        {
            Debug.LogWarning("No sanctuary positions available.");
            return null; // ��^�šA�קK���~
        }

        List<Transform> SanctuaryPosition = new List<Transform>(onAccidentPositions);

        // ���PNPC���Z���Ƨ�
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
