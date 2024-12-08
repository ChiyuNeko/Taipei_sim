using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniGLTF.Extensions.VRMC_springBone;
using Unity.VisualScripting;
using UnityEngine;

public class IceBall : MonoBehaviour
{
    public DisasterFactorySO pool;

    private bool isExploded;
    private Rigidbody rb;
    public SphereCollider SpCollider;
    public GameObject AccidentAlarmArea;//觸發市民isAccident的範圍
    public GameObject OriginalIceBall;//墜落時的物件
    public GameObject IceBallScrap;//爆炸粒子特效

    public Vector3 Force;

    private void OnEnable()
    {
        isExploded = false;
        AccidentAlarmArea.SetActive(false);
        OriginalIceBall.SetActive(true);
        IceBallScrap.SetActive(false);

        rb = GetComponent<Rigidbody>();
        rb.AddForce(Force);

        Invoke(nameof(DisableSelf), 10f);
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
    }

    async Task Bigger()
    {
        while (isExploded)
        {
            SpCollider.radius += 0.5f;
            await Task.Yield();
        } 
        await Task.Yield();
    }

    void ExplodeIceBall()
    {
        isExploded = true;
        AccidentAlarmArea.SetActive(true);
        OriginalIceBall.SetActive(false);
        IceBallScrap.SetActive(true);
    }

    void DisableSelf()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        isExploded = false;
        SpCollider.radius = 0.5f;
        pool.Return(gameObject);
    }
}
