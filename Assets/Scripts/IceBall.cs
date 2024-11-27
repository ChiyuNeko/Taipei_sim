using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : MonoBehaviour
{
    private bool isExploded;

    private Rigidbody rb;

    public GameObject OriginalIceBall;//墜落時的物件
    public GameObject IceBallScrap;//爆炸粒子特效
    public GameObject AccidentAlarmArea;//觸發市民isAccident的範圍

    public Vector3 Force;
    void Start()
    {
        isExploded = false;
        AccidentAlarmArea.SetActive(false);
        OriginalIceBall.SetActive(true);
        IceBallScrap.SetActive(false);

        rb = GetComponent<Rigidbody>();
        rb.AddForce(Force);
    }
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isExploded)
        {
            ExplodeIceBall();
        }
    }

    void ExplodeIceBall()
    {
        isExploded = true;
        AccidentAlarmArea.SetActive(true);
        OriginalIceBall.SetActive(false);
        IceBallScrap.SetActive(true);
    }
}
