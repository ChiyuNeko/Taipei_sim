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
    private NavMeshAgent agent;  // NPC��NavMeshAgent
    public List<Transform> destinations = new List<Transform>();  // �s���H��������ت��a

    //public float waitTime = 3.0f;  // ���d�ɶ�
    public float DailyMoveSpeed = 2f;

    private BoxCollider Bcollider;

    private int currentDestinationIndex = 0;  // �ثe���ت��a����
    private float destinationRadius = 0.5f;  // �վ��F�P�w�d��
    private int citizensSideWalkAreaMask;  // �x�scitizensSideWalk���ϰ챻�X
    private int citizensWalkableMask;

    public bool isAccident = false;  // �O�_�o�ͨƬG
    public bool showUI = false;
    void Start()
    {
        if(NPCsManager == null)
        {
            NPCsManager = GameObject.Find("SceneManger").GetComponent<NPCsManager>();
        }

        Bcollider = GetComponent<BoxCollider>();
        agent = GetComponent<NavMeshAgent>();
        citizenAnimation = GetComponent<Animator>();
        aUI = transform.GetChild(0).GetComponent<ActorUI>();

        destinationRadius = 1f;
        //DailyMoveSpeed = DailyMoveSpeed + Random.Range(-0.5f, 0.5f);
        DailyMoveSpeed = Random.Range(0.5f, 3f);
        citizenAnimation.SetFloat("dailyMoveSpeed", MapToRange(DailyMoveSpeed, 0.5f, 3f, 1, 5));
        agent.speed = DailyMoveSpeed;

        citizensWalkableMask = 1 << NavMesh.GetAreaFromName("citizensWalkableMask");
        citizensSideWalkAreaMask = 2 << NavMesh.GetAreaFromName("citizensSideWalk");// ���citizensSideWalk���ϰ챻�X

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
            aUI.DBugLog(agent.pathPending);
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
        if (triggerTag == "CitizensTargetPosition")
        {

        }
    }

    int MapToRange(float value, float min1, float max1, float min2, float max2)
    {
        // �u�ʬM�g�����å|�ˤ��J
        float mappedFloat = (value - min1) * (max2 - min2) / (max1 - min1) + min2;
        return Mathf.RoundToInt(mappedFloat); // �|�ˤ��J�����
    }

    void DestinationDetect()
    {
        aUI.ShowDestination(agent.remainingDistance.ToString());
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

        // ����citizensSideWalk�ϰ챻�X
        agent.areaMask &= ~citizensSideWalkAreaMask;
        StartCoroutine(WaitPanicAnimation());
        
        agent.SetDestination(NPCsManager.GetClosestSanctuaryPosition(gameObject.transform).position);
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
    */
}