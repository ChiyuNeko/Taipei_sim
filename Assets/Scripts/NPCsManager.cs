using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCsManager : MonoBehaviour
{
    NPCSpawner spawner;
    public List<GameObject> NPCs = new List<GameObject>();


    bool startTrigger = false;

    void Start()
    {
        spawner = GetComponent<NPCSpawner>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && startTrigger!= true)
        {
            GameSet();
            startTrigger = !startTrigger;
        }
    }

    void GameSet()
    {
        spawner.SpawnNPC(50);
    }

    public Transform GetClosestSanctuaryPosition(Transform NPC)
    {
        return spawner.GetClosestSanctuaryPosition(NPC);
    }
    public Transform GetRandomDestinations()
    {
        return spawner.GetRandomDestinations();
    }
}
