using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public List<GameObject> npcPrefabs;  // �x�s�Ҧ���NPC prefab
    public Transform spawnPointParent;   // NPC�ͦ���m�������� (spawnPoint_citizens)
    public float spawnDelayTime = 0.2f;    // �ͦ�����ɶ�
    public int maxNPCCount = 10;         // ����ͦ���NPC�ƶq

    private List<Transform> spawnPoints = new List<Transform>();  // �x�s�Ҧ��l���󪺥ͦ��I
    private int currentNPCCount = 0;



    void Start()
    {
        // ��l�ƩҦ��ͦ��I
        foreach (Transform spawnPoint in spawnPointParent)
        {
            spawnPoints.Add(spawnPoint);
        }

        // �}�l�۰ʥͦ�NPC
        StartCoroutine(SpawnNPCs());
    }

    IEnumerator SpawnNPCs()
    {
        while (currentNPCCount < maxNPCCount)
        {
            // �H����ܤ@��NPC prefab
            GameObject npcPrefab = npcPrefabs[Random.Range(0, npcPrefabs.Count)];

            // �H����ܤ@�ӥͦ��I
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            // �ͦ�NPC
            Instantiate(npcPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);

            // �W�[�ͦ���NPC�ƶq
            currentNPCCount++;

            // ���ݤU�@���ͦ�
            yield return new WaitForSeconds(spawnDelayTime);
        }
    }
}
