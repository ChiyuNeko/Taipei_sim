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
    private NavMeshAgent agent;  // NPC��NavMeshAgent
    private Transform targetPositionCollection;  // �]�t�Ҧ��ت��a��������
    public List<Transform> destinations = new List<Transform>();  // �s���H��������ت��a
    public Transform sanctuaryPosition;  // ��isAccident��true�ɪ��ؼЦ�m
    public float waitTime = 3.0f;  // ���d�ɶ�
    public float DailyMoveSpeed = 2f;

    private BoxCollider Bcollider;

    private int currentDestinationIndex = 0;  // �ثe���ت��a����
    private bool waiting = false;  // �O�_�b����
    private bool hasArrived = false;  // �O�_�w�g��F�ت��a
    private float destinationRadius = 0.5f;  // �վ��F�P�w�d��
    private int citizensSideWalkAreaMask;  // �x�scitizensSideWalk���ϰ챻�X

    public bool isAccident = false;  // �O�_�o�ͨƬG
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
        citizensSideWalkAreaMask = 1 << NavMesh.GetAreaFromName("citizensSideWalk");// ���citizensSideWalk���ϰ챻�X

        switch (action)
        {
            case Action.Moving:
                citizenAnimation.SetInteger("ActionID",0);
                SelectRandomDestinations();// �H���qtargetPosition���U���l���󤤿��5�ӥت��a
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

        // ����citizensSideWalk�ϰ챻�X
        agent.areaMask &= ~citizensSideWalkAreaMask;
        StartCoroutine(WaitPanicAnimation());
        aUI.DBugLog("onAccident");
        setMyDestination(sanctuaryPosition.position);
    }

    void disAccident()
    {
        isAccident = false;
        citizenAnimation.SetBool("isAccident", isAccident);

        // �[�^citizensSideWalk�ϰ챻�X
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

    //  ��F�ت��a��𮧴X��A�e���U�@�ӥت��a
    IEnumerator WaitAtDestination()
    {
        aUI.DBugLog("Waiting...");
        waiting = true;
        citizenAnimation.SetBool("isStanding", true);
        yield return new WaitForSeconds(waitTime);  // ���d3��
        citizenAnimation.SetBool("isStanding", false);

        // �e���U�@�ӥت��a
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

        // �����Ҧ��ت��a
        foreach (Transform child in targetPositionCollection)
        {
            allDestinations.Add(child);
        }

        // ���PNPC���Z���Ƨ�
        allDestinations.Sort((a, b) =>
            Vector3.Distance(transform.position, a.position).CompareTo(
                Vector3.Distance(transform.position, b.position)
            )
        );

        // ����̪񪺤���
        int destinationCount = Mathf.Min(5, allDestinations.Count); // �T�O�̦h�u��5��
        for (int i = 0; i < destinationCount; i++)
        {
            destinations.Add(allDestinations[i]);
        }
    }

}
