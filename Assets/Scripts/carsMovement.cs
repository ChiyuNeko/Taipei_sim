using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class carsMovement : MonoBehaviour
{
    public NavMeshAgent agent;  // NPC��NavMeshAgent
    public Transform targetPosition;  // �]�t�Ҧ��ت��a��������
    public List<Transform> destinations = new List<Transform>();  // �s���H��������ت��a

    public float waitTime = 3.0f;  // ���d�ɶ�

    private int currentDestinationIndex = 0;  // �ثe���ت��a����
    private bool waiting = false;  // �O�_�b����
    private bool hasArrived = false;  // �O�_�w�g��F�ت��a
    private float destinationRadius = 1.0f;  // �վ��F�P�w�d��

    // Start is called before the first frame update
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        agent.speed = 9f;

        // �H���qtargetPosition���U���l���󤤿��5�ӥت��a
        SelectRandomDestinations();

        if (destinations.Count > 0)
        {
            // �]�w�Ĥ@�ӥت��a
            agent.SetDestination(destinations[currentDestinationIndex].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
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

    IEnumerator WaitAtDestination()
    {
        waiting = true;
        yield return new WaitForSeconds(waitTime);  // ���d3��

        // �e���U�@�ӥت��a
        currentDestinationIndex = (currentDestinationIndex + 1) % destinations.Count;
        agent.SetDestination(destinations[currentDestinationIndex].position);

        hasArrived = false;
        waiting = false;
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
