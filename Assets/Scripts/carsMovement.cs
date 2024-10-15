using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class carsMovement : MonoBehaviour
{
    public NavMeshAgent agent;  // NPC的NavMeshAgent
    public Transform targetPosition;  // 包含所有目的地的父物件
    public List<Transform> destinations = new List<Transform>();  // 存放隨機抽取的目的地

    public float waitTime = 3.0f;  // 停留時間

    private int currentDestinationIndex = 0;  // 目前的目的地索引
    private bool waiting = false;  // 是否在等待
    private bool hasArrived = false;  // 是否已經抵達目的地
    private float destinationRadius = 1.0f;  // 調整抵達判定範圍

    // Start is called before the first frame update
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        agent.speed = 9f;

        // 隨機從targetPosition底下的子物件中選取5個目的地
        SelectRandomDestinations();

        if (destinations.Count > 0)
        {
            // 設定第一個目的地
            agent.SetDestination(destinations[currentDestinationIndex].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 調整抵達判定條件：當距離小於destinationRadius時，視為抵達
        if (!waiting && agent.remainingDistance <= destinationRadius && !agent.pathPending)
        {
            if (!hasArrived)
            {
                hasArrived = true;
                StartCoroutine(WaitAtDestination());
            }
        }
    }

    IEnumerator WaitAtDestination()
    {
        waiting = true;
        yield return new WaitForSeconds(waitTime);  // 停留3秒

        // 前往下一個目的地
        currentDestinationIndex = (currentDestinationIndex + 1) % destinations.Count;
        agent.SetDestination(destinations[currentDestinationIndex].position);

        hasArrived = false;
        waiting = false;
    }

    // 從targetPosition底下隨機選擇5個子物件作為目的地
    void SelectRandomDestinations()
    {
        List<Transform> allDestinations = new List<Transform>();

        foreach (Transform child in targetPosition)
        {
            allDestinations.Add(child);
        }

        for (int i = 0; i < 5 && allDestinations.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, allDestinations.Count);
            destinations.Add(allDestinations[randomIndex]);
            allDestinations.RemoveAt(randomIndex);  // 移除已選中的，避免重複選擇
        }
    }



}
