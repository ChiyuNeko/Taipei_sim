using System.Collections.Generic;
using UnityEngine;

public class CameraCitizenTracker : MonoBehaviour
{
    public Transform nowTrackingNPC;

    public List<Transform> citizens = new List<Transform>();
    public Transform cameraTarget;  // 相機
    public float updateInterval = 1f;  // 更新頻率（秒）
    private float timer = 0f;
    private int currentCitizenIndex = 0;  // 當前追蹤的NPC索引



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

        // 切換下一個或上一個NPC
        if (citizens.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentCitizenIndex = (currentCitizenIndex + 1) % citizens.Count;  // 循環到下一個NPC
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentCitizenIndex = (currentCitizenIndex - 1 + citizens.Count) % citizens.Count;  // 循環到上一個NPC
            }

            // 確保相機目標時時更新為當前追蹤的NPC位置
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

        // 如果目前索引超出範圍，重置索引
        if (currentCitizenIndex >= citizens.Count)
        {
            currentCitizenIndex = 0;
        }
    }

    void UpdateCameraTarget()
    {
        if (citizens.Count > 0)
        {
            // 讓 cameraTarget 的位置時時更新為當前的 NPC 位置
            cameraTarget.position = citizens[currentCitizenIndex].position;
            nowTrackingNPC = citizens[currentCitizenIndex];
        }
    }
}
