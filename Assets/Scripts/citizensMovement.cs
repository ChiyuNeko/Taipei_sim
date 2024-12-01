using System.Collections;
using System.Collections.Generic;
using UniGLTF.Extensions.VRMC_springBone;
using UnityEngine;
using UnityEngine.AI;
using Collider = UnityEngine.Collider;

public class citizensMovement : MonoBehaviour
{
    public enum Action
    {
        Moving,
        setTalikig1,
        setTalikig2,
        setTalikig3,
        setTalikig4,
        standTalk1,
    }

    public Action action;
    public ActorUI aUI;
    private Animator citizenAnimation;
    private NavMeshAgent agent;  // NPC的NavMeshAgent
    private Transform targetPositionCollection;  // 包含所有目的地的父物件
    public List<Transform> destinations = new List<Transform>();  // 存放隨機抽取的目的地
    public Transform sanctuaryPosition;  // 當isAccident為true時的目標位置
    public float waitTime = 3.0f;  // 停留時間
    public float DailyMoveSpeed = 2f;

    private BoxCollider Bcollider;

    private int currentDestinationIndex = 0;  // 目前的目的地索引
    private bool waiting = false;  // 是否在等待
    private bool hasArrived = false;  // 是否已經抵達目的地
    private float destinationRadius = 0.5f;  // 調整抵達判定範圍
    private int citizensSideWalkAreaMask;  // 儲存citizensSideWalk的區域掩碼

    public bool isAccident = false;  // 是否發生事故
    public bool ImDown = false;
    public bool showUI = false;
    void Start()
    {
        Bcollider = GetComponent<BoxCollider>();
        agent = GetComponent<NavMeshAgent>();
        citizenAnimation = GetComponent<Animator>();
        aUI = transform.GetChild(0).GetComponent<ActorUI>();
        targetPositionCollection = GameObject.Find("CitizenDailyPosition").GetComponent<Transform>();
        sanctuaryPosition = GameObject.Find("onAccidentPosition_Pos").GetComponent<Transform>();

        aUI.DBugLog(">>START ");
        aUI.DBugLog("GetComponent Finish");

        destinationRadius = 1f;
        DailyMoveSpeed = DailyMoveSpeed + Random.Range(-0.5f, 0.5f);
        ImDown = false;

        agent.speed = DailyMoveSpeed;
        citizensSideWalkAreaMask = 1 << NavMesh.GetAreaFromName("citizensSideWalk");// 獲取citizensSideWalk的區域掩碼

        switch (action)
        {
            case Action.Moving:
                citizenAnimation.SetInteger("ActionID",0);
                SelectRandomDestinations();// 隨機從targetPosition底下的子物件中選取5個目的地
                agent.enabled = true;
                setMyDestination(gameObject.transform.position);
                break;
            case Action.setTalikig1:
                citizenAnimation.SetInteger("ActionID", 1);
                break;
            case Action.setTalikig2:
                citizenAnimation.SetInteger("ActionID", 2);
                break;
            case Action.setTalikig3:
                citizenAnimation.SetInteger("ActionID", 3);
                break;
            case Action.setTalikig4:
                citizenAnimation.SetInteger("ActionID", 4);
                break;
            case Action.standTalk1:
                citizenAnimation.SetInteger("ActionID", 5);
                break;
            default: 
                break;
        }

        if(showUI)
        {
            aUI.gameObject.SetActive(true);
        }
        else
        {
            aUI.gameObject.SetActive(false);
        }
        
    }

    void Update()
    {
        if(action == Action.Moving)
        {
            if (!isAccident)
            {
                DestinationDetect();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        string collisionTag = collision.gameObject.tag;
        if (collisionTag == "hug-IceBall" || collisionTag == "Car" || collisionTag == "brokenBuild")
        {
            OnDown();
            aUI.DBugLog("hit By " + collisionTag);
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
        agent.enabled = true;
        citizenAnimation.SetBool("isAccident", isAccident);

        // 移除citizensSideWalk區域掩碼
        agent.areaMask &= ~citizensSideWalkAreaMask;
        StartCoroutine(WaitPanicAnimation());
        aUI.DBugLog("onAccident");
        setMyDestination(sanctuaryPosition.position);
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
        Bcollider.isTrigger = true;
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
        aUI.DBugLog("Waiting...");
        waiting = true;
        citizenAnimation.SetBool("isStanding", true);
        yield return new WaitForSeconds(waitTime);  // 停留3秒
        citizenAnimation.SetBool("isStanding", false);

        // 前往下一個目的地
        currentDestinationIndex = (currentDestinationIndex + 1) % destinations.Count;
        setMyDestination(destinations[currentDestinationIndex].position);

        hasArrived = false;
        waiting = false;
    }

    IEnumerator WaitPanicAnimation()
    {
        agent.speed = 0f;
        yield return new WaitForSeconds(1.5f);
        agent.speed = 5f + Random.Range(-1f, 1f); ;
    }

    void setMyDestination(Vector3 position)
    {
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent is not assigned!");
            return;
        }

        NavMeshHit hit;
        if (!NavMesh.SamplePosition(position, out hit, 1f, NavMesh.AllAreas))
        {
            Debug.LogError("Destination is not on NavMesh: " + position);
            return;
        }

        if (agent.isOnNavMesh)
        {
            agent.SetDestination(position);
            aUI.DBugLog("Set New Pos " + position.ToString());
            aUI.ShowDestination(agent.remainingDistance.ToString());
        }
        else
        {
            Debug.LogError("Agent is not on NavMesh!");
        }
    }


    void SelectRandomDestinations()
    {
        List<Transform> allDestinations = new List<Transform>();

        // 收集所有目的地
        foreach (Transform child in targetPositionCollection)
        {
            allDestinations.Add(child);
        }

        // 按與NPC的距離排序
        allDestinations.Sort((a, b) =>
            Vector3.Distance(transform.position, a.position).CompareTo(
                Vector3.Distance(transform.position, b.position)
            )
        );

        // 選取最近的五個
        int destinationCount = Mathf.Min(5, allDestinations.Count); // 確保最多只取5個
        for (int i = 0; i < destinationCount; i++)
        {
            destinations.Add(allDestinations[i]);
        }
    }

}
