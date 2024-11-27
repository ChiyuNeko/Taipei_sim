using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class citizensMovement : MonoBehaviour
{
    private Animator citizenAnimation;

    public NavMeshAgent agent;  // NPC的NavMeshAgent
    private Transform targetPositionCollection;  // 包含所有目的地的父物件
    private List<Transform> destinations = new List<Transform>();  // 存放隨機抽取的目的地
    private Transform sanctuaryPosition;  // 當isAccident為true時的目標位置
    public float waitTime = 3.0f;  // 停留時間
    public float DailyMoveSpeed = 2f;

    private int currentDestinationIndex = 0;  // 目前的目的地索引
    private bool waiting = false;  // 是否在等待
    private bool hasArrived = false;  // 是否已經抵達目的地
    private float destinationRadius = 0.5f;  // 調整抵達判定範圍
    private int citizensSideWalkAreaMask;  // 儲存citizensSideWalk的區域掩碼

    public bool isAccident = false;  // 是否發生事故
    public bool ImDown = false;

    void Start()
    {
        DailyMoveSpeed = DailyMoveSpeed + Random.Range(-1f, 1f);
        ImDown = false;

        citizenAnimation = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = DailyMoveSpeed;

        targetPositionCollection = GameObject.Find("CitizenDailyPosition").GetComponent<Transform>();
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
        if (!isAccident)
        {
            DestinationDetect();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        string collisionTag = collision.gameObject.tag;
        if (collisionTag == "hug-IceBall" || collisionTag == "Car" || collisionTag == "brokenBuild")
        {
            OnDown();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        string triggerTag = other.gameObject.tag;
        if (triggerTag == "AccidentAlarm" && isAccident == false)
        {
            onAccident();
        }
    }

    void DestinationDetect()
    {
        if (!waiting && agent.remainingDistance <= destinationRadius && !agent.pathPending)
        {
            if (!hasArrived)
            {
                hasArrived = true;
                StartCoroutine(WaitAtDestination());
            }
        }
    }

    void onAccident()
    {
        isAccident = true;
        citizenAnimation.SetBool("isAccident", isAccident);

        // 移除citizensSideWalk區域掩碼
        agent.areaMask &= ~citizensSideWalkAreaMask;
        agent.SetDestination(sanctuaryPosition.position);
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

    void OnDown()
    {
        ImDown = true;
        citizenAnimation.SetBool("isDown", true);
        agent.speed = 0.3f;
        StartCoroutine(DeathCounter(3));
    }

    IEnumerator DeathCounter(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
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
        agent.speed = 5f + Random.Range(-1f, 1f); ;
    }

    // 從targetPosition底下隨機選擇5個子物件作為目的地
    void SelectRandomDestinations()
    {
        List<Transform> allDestinations = new List<Transform>();

        foreach (Transform child in targetPositionCollection)
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
