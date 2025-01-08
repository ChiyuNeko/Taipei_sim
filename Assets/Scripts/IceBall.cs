using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UniGLTF.Extensions.VRMC_springBone;
using Unity.VisualScripting;
using UnityEngine;

public class IceBall : MonoBehaviour
{
    public DisasterFactorySO pool;

    public bool isExploded;
    private Rigidbody rb;
    public SphereCollider SpCollider;
    public GameObject AccidentAlarmArea;//觸發市民isAccident的範圍
    public GameObject OriginalIceBall;//墜落時的物件

    public GameObject IceBallScrap1;//爆炸粒子特效
    public GameObject IceBallScrap2;//爆炸粒子特效
    public GameObject IceBallScrap3;//爆炸粒子特效


    public Vector3 Force;

    private void Update()
    {
        if(gameObject.transform.position.y <= -500)
        {
            pool.Return(gameObject);
        }
    }

    private void OnEnable()
    {
        isExploded = false;
        AccidentAlarmArea.SetActive(false);
        OriginalIceBall.SetActive(true);
        IceBallScrap1.SetActive(false);
        IceBallScrap2.SetActive(false);
        IceBallScrap3.SetActive(false);

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(Force);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isExploded)
        {
            ExplodeIceBall();
            OnFloor();
        }
    }
    
    async void OnFloor()
    {
        await Bigger();
        rb.isKinematic = true;
    }

    async Task Bigger()
    {
        float LiveTime = 3f;
        float nowTime = 0;
        while (isExploded && nowTime<=LiveTime)
        {
            SpCollider.radius += 0.2f;
            nowTime += Time.deltaTime;
            await Task.Yield();
        }
        pool.Return(gameObject);
    }

    void ExplodeIceBall()
    {
        isExploded = true;
        AccidentAlarmArea.SetActive(true);
        OriginalIceBall.SetActive(false);
        IceBallScrap1.SetActive(true);
        IceBallScrap2.SetActive(true);
        IceBallScrap3.SetActive(true);

        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    private void OnDisable()
    {
        isExploded = false;
        SpCollider.radius = 0.5f;
        pool.Return(gameObject);
    }
}
