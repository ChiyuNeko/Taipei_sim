using System.Collections.Generic;
using UnityEngine;

public class CameraCitizenTracker : MonoBehaviour
{
    public Transform nowTrackingNPC;

    public List<Transform> citizens = new List<Transform>();
    public Transform cameraTarget;  // �۾�
    public float updateInterval = 1f;  // ��s�W�v�]���^
    public Vector3 offset;
    private float timer = 0f;
    private int currentCitizenIndex = 0;  // ���e�l�ܪ�NPC����



    void Start()
    {
        UpdateCitizenList();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            UpdateCitizenList();
            timer = 0f;
        }

        // �����U�@�өΤW�@��NPC
        if (citizens.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentCitizenIndex = (currentCitizenIndex + 1) % citizens.Count;  // �`����U�@��NPC
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentCitizenIndex = (currentCitizenIndex - 1 + citizens.Count) % citizens.Count;  // �`����W�@��NPC
            }

            // �T�O�۾��ؼЮɮɧ�s�����e�l�ܪ�NPC��m
            UpdateCameraTarget();
        }
    }

    void UpdateCitizenList()
    {
        citizens.Clear();
        GameObject[] citizenObjects = GameObject.FindGameObjectsWithTag("Citizens");
        foreach (GameObject citizen in citizenObjects)
        {
            citizens.Add(citizen.transform);
        }

        // �p�G�ثe���޶W�X�d��A���m����
        if (currentCitizenIndex >= citizens.Count)
        {
            currentCitizenIndex = 0;
        }
    }

    void UpdateCameraTarget()
    {
        if (citizens.Count > 0)
        {
            // �� cameraTarget ����m�ɮɧ�s�����e�� NPC ��m
            cameraTarget.position = citizens[currentCitizenIndex].position + offset;
            nowTrackingNPC = citizens[currentCitizenIndex];
        }
    }
}
