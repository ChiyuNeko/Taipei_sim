using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class citizensMovement : MonoBehaviour
{
    private Animator citizenAnimation;

    public NavMeshAgent agent;  // NPC��NavMeshAgent
    private Transform targetPositionCollection;  // �]�t�Ҧ��ت��a��������
    private List<Transform> destinations = new List<Transform>();  // �s���H��������ت��a
    private Transform sanctuaryPosition;  // ��isAccident��true�ɪ��ؼЦ�m
    public float waitTime = 3.0f;  // ���d�ɶ�
    public float DailyMoveSpeed = 2f;

    private int currentDestinationIndex = 0;  // �ثe���ت��a����
    private bool waiting = false;  // �O�_�b����
    private bool hasArrived = false;  // �O�_�w�g��F�ت��a
    private float destinationRadius = 0.5f;  // �վ��F�P�w�d��
    private int citizensSideWalkAreaMask;  // �x�scitizensSideWalk���ϰ챻�X

    public bool isAccident = false;  // �O�_�o�ͨƬG
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

        // ���citizensSideWalk���ϰ챻�X
        citizensSideWalkAreaMask = 1 << NavMesh.GetAreaFromName("citizensSideWalk");

        // �H���qtargetPosition���U���l���󤤿��5�ӥت��a
        SelectRandomDestinations();

        if (destinations.Count > 0)
        {
            // �]�w�Ĥ@�ӥت��a
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

        // ����citizensSideWalk�ϰ챻�X
        agent.areaMask &= ~citizensSideWalkAreaMask;
        agent.SetDestination(sanctuaryPosition.position);
        StartCoroutine(WaitPanicAnimation());
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
        waiting = true;
        citizenAnimation.SetBool("isStanding", true);
        yield return new WaitForSeconds(waitTime);  // ���d3��
        citizenAnimation.SetBool("isStanding", false);

        // �e���U�@�ӥت��a
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

    // �qtargetPosition���U�H�����5�Ӥl����@���ت��a
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
            allDestinations.RemoveAt(randomIndex);  // �����w�襤���A�קK���ƿ��
        }
    }
}
