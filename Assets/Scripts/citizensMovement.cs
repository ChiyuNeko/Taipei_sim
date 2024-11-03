using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class citizensMovement : MonoBehaviour
{
    public Animator citizenAnimation;

    public NavMeshAgent agent;  // NPC��NavMeshAgent
    public Transform targetPosition;  // �]�t�Ҧ��ت��a��������
    public List<Transform> destinations = new List<Transform>();  // �s���H��������ت��a
    public Transform sanctuaryPosition;  // ��isAccident��true�ɪ��ؼЦ�m
    public float waitTime = 3.0f;  // ���d�ɶ�
    public bool isAccident = false;  // �O�_�o�ͨƬG
    public float DailyMoveSpeed = 2f;

    private int currentDestinationIndex = 0;  // �ثe���ت��a����
    private bool waiting = false;  // �O�_�b����
    private bool hasArrived = false;  // �O�_�w�g��F�ت��a
    private float destinationRadius = 1.0f;  // �վ��F�P�w�d��

    private int citizensSideWalkAreaMask;  // �x�scitizensSideWalk���ϰ챻�X

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
            // �p�GisAccident��true�A�ߨ�e��sanctuaryPosition����m
            agent.SetDestination(sanctuaryPosition.position);
            return;
        }

        // �վ��F�P�w����G��Z���p��destinationRadius�ɡA������F
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

        // ����citizensSideWalk�ϰ챻�X
        agent.areaMask &= ~citizensSideWalkAreaMask;

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
        agent.speed = 5f;
    }

    // �qtargetPosition���U�H�����5�Ӥl����@���ت��a
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
            allDestinations.RemoveAt(randomIndex);  // �����w�襤���A�קK���ƿ��
        }
    }
}
