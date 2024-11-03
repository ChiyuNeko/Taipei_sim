using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class citizensMovement : MonoBehaviour
{
    public Animator citizenAnimation;

    public NavMeshAgent agent;  // NPC的NavMeshAgent
    public Transform targetPosition;  // 包含所有目的地的父物件
    public List<Transform> destinations = new List<Transform>();  // 存放隨機抽取的目的地
    public Transform sanctuaryPosition;  // 當isAccident為true時的目標位置
    public float waitTime = 3.0f;  // 停留時間
    public bool isAccident = false;  // 是否發生事故
    public float DailyMoveSpeed = 2f;

    private int currentDestinationIndex = 0;  // 目前的目的地索引
    private bool waiting = false;  // 是否在等待
    private bool hasArrived = false;  // 是否已經抵達目的地
    private float destinationRadius = 1.0f;  // 調整抵達判定範圍

    private int citizensSideWalkAreaMask;  // 儲存citizensSideWalk的區域掩碼

    bool ImDown = false;

    void Start()
    {
        DailyMoveSpeed = DailyMoveSpeed + Random.Range(-0.5f, 0.5f);
        ImDown = false;

        citizenAnimation = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = DailyMoveSpeed;

        targetPosition = GameObject.Find("CitizenDailyPosition").GetComponent<Transform>();
        sanctuaryPosition = GameObject.Find("onAccidentPosition_Pos").GetComponent<Transform>();

        // 獲取citizensSideWalk的區域掩碼
        citizensSideWalkAreaMask = 1 << NavMesh.GetAreaFromName("citizensSideWalk");

        // 隨機從targetPosition底下的子物件中選取5個目的地
        SelectRandomDestinations();

        if (destinations.Count > 0)
        {
            // 設定第一個目的地
            agent.SetDestination(destinations[currentDestinationIndex].position);
        }
    }

    void Update()
    {

        if (ImDown)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            onAccident();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            disAccident();
        }

        if (isAccident)
        {
            // 如果isAccident為true，立刻前往sanctuaryPosition的位置
            agent.SetDestination(sanctuaryPosition.position);
            return;
        }

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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "brokenBuild")
        {
            ImDown = true;
            citizenAnimation.SetBool("isDown", true);
            agent.speed = 0.3f;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "brokenBuild")
        {
            ImDown = true;
            citizenAnimation.SetBool("isDown", true);
            agent.speed = 0.3f;
        }
    }

    void onAccident()
    {
        isAccident = true;
        citizenAnimation.SetBool("isAccident", isAccident);

        // 移除citizensSideWalk區域掩碼
        agent.areaMask &= ~citizensSideWalkAreaMask;

        StartCoroutine(WaitPanicAnimation());
    }

    void disAccident()
    {
        isAccident = false;
        citizenAnimation.SetBool("isAccident", isAccident);

        // 加回citizensSideWalk區域掩碼
        agent.areaMask |= citizensSideWalkAreaMask;

        agent.SetDestination(destinations[currentDestinationIndex].position);
        agent.speed = DailyMoveSpeed;
    }



    //  抵達目的地後休息幾秒再前往下一個目的地
    IEnumerator WaitAtDestination()
    {
        waiting = true;
        citizenAnimation.SetBool("isStanding", true);
        yield return new WaitForSeconds(waitTime);  // 停留3秒
        citizenAnimation.SetBool("isStanding", false);

        // 前往下一個目的地
        currentDestinationIndex = (currentDestinationIndex + 1) % destinations.Count;
        agent.SetDestination(destinations[currentDestinationIndex].position);

        hasArrived = false;
        waiting = false;
    }

    IEnumerator WaitPanicAnimation()
    {
        agent.speed = 0f;
        yield return new WaitForSeconds(1.5f);
        agent.speed = 5f;
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
