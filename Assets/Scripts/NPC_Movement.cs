using System.Collections;
using System.Collections.Generic;
using UniGLTF.Extensions.VRMC_springBone;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Collider = UnityEngine.Collider;

public class NPC_Movement : MonoBehaviour
{
    public NPCsManager NPCsManager;

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
    public List<Transform> destinations = new List<Transform>();  // 存放隨機抽取的目的地

    public float knockbackForce = 20f;  // 被撞飛的力道

    //public float waitTime = 3.0f;  // 停留時間
    public float DailyMoveSpeed = 2f;

    private BoxCollider boxCllider;

    private int currentDestinationIndex = 0;  // 目前的目的地索引
    private float destinationRadius = 0.5f;  // 調整抵達判定範圍
    private int citizensSideWalkAreaMask;  // 儲存citizensSideWalk的區域掩碼
    private int citizensWalkableMask;

    public bool isAccident = false;  // 是否發生事故
    public bool showUI = false;

    private void Awake()
    {
        if (NPCsManager == null)
        {
            NPCsManager = GameObject.FindGameObjectWithTag("GameSceneManager").GetComponent<NPCsManager>();
        }
    }
    void Start()
    {
        boxCllider = GetComponent<BoxCollider>();
        agent = GetComponent<NavMeshAgent>();
        citizenAnimation = GetComponent<Animator>();
        //aUI = transform.GetChild(0).GetComponent<ActorUI>();

        destinationRadius = 1f;
        //DailyMoveSpeed = DailyMoveSpeed + Random.Range(-0.5f, 0.5f);
        DailyMoveSpeed = Random.Range(0.5f, 3f);
        citizenAnimation.SetFloat("dailyMoveSpeed", MapToRange(DailyMoveSpeed, 0.5f, 3f, 1, 5));
        agent.speed = DailyMoveSpeed;

        citizensWalkableMask = 1 << NavMesh.GetAreaFromName("citizensWalkableMask");
        citizensSideWalkAreaMask = 2 << NavMesh.GetAreaFromName("citizensSideWalk");// 獲取citizensSideWalk的區域掩碼

        switch (action)
        {
            case Action.Moving:
                citizenAnimation.SetInteger("ActionID",0);
                agent.enabled = true;
                agent.areaMask &= ~citizensWalkableMask;
                setNextPosistion();
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
            //aUI.gameObject.SetActive(true);
        }
        else
        {
            //aUI.gameObject.SetActive(false);
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
            //aUI.DBugLog(agent.pathPending);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        string collisionTag = collision.gameObject.tag;
        if (collisionTag == "hug-IceBall" || collisionTag == "Car" || collisionTag == "brokenBuild")
        {
            OnDown();
        }
        if (collisionTag == "Cars")
        {
            OnCarCrash(collision);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        string triggerTag = other.gameObject.tag;
        if (triggerTag == "AccidentAlarm" && isAccident == false)
        {
            onAccident();
        }
        if (triggerTag == "CitizensTargetPosition")
        {

        }
    }

    int MapToRange(float value, float min1, float max1, float min2, float max2)
    {
        // 線性映射公式並四捨五入
        float mappedFloat = (value - min1) * (max2 - min2) / (max1 - min1) + min2;
        return Mathf.RoundToInt(mappedFloat); // 四捨五入取整數
    }

    void DestinationDetect()
    {
        //aUI.ShowDestination(agent.remainingDistance.ToString());
        if (agent.remainingDistance <= destinationRadius && !agent.pathPending)
        {
            setNextPosistion();
        }
    }

    void setNextPosistion()
    {
        Transform nextPosistion = NPCsManager.GetRandomDestinations();
        agent.SetDestination(nextPosistion.position);
    }

    void onAccident()
    {
        isAccident = true;
        agent.enabled = true;
        citizenAnimation.SetBool("isAccident", isAccident);

        // 移除citizensSideWalk區域掩碼
        agent.areaMask &= ~citizensSideWalkAreaMask;
        StartCoroutine(WaitPanicAnimation());
        
        agent.SetDestination(NPCsManager.GetClosestSanctuaryPosition(gameObject.transform).position);
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

    private void OnDown()
    {
        boxCllider.isTrigger = true;
        citizenAnimation.SetBool("isDown", true);
        agent.speed = 0.3f;
        StartCoroutine(DeathCounter(3));
    }
    private void OnCarCrash(Collision collision)
    {
        boxCllider.isTrigger = true;
        citizenAnimation.SetBool("isDown", true);
        agent.speed = 0.3f;

        // 計算撞擊方向
        Vector3 crashDirection = (transform.position - collision.transform.position).normalized;

        // 增加一個向上的分量
        Vector3 knockbackDirection = crashDirection + Vector3.up * 0.8f;  // 垂直方向的強度可以調整

        // 施加反方向的力
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();  // 如果沒有Rigidbody則添加
        }
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

        StartCoroutine(DeathCounter(3));
    }


    IEnumerator DeathCounter(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
    IEnumerator WaitPanicAnimation()
    {
        agent.speed = 0f;
        yield return new WaitForSeconds(1.5f);
        agent.speed = 5f + Random.Range(-1f, 1f); ;
    }

    /*
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
    */
}
