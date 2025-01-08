using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class meteorite : MonoBehaviour
{
    public DisasterFactorySO pool;

    public bool isExploded;
    private Rigidbody rb;
    public SphereCollider SpCollider;
    public GameObject AccidentAlarmArea;//牟祇カチisAccident絛瞅
    public GameObject OriginalIceBall;//糦辅ン

    public GameObject IceBallScrap1;//脄采疭
    public GameObject IceBallScrap2;//脄采疭
    public GameObject IceBallScrap3;//脄采疭
    public GameObject IceBallScrap4;//脄采疭
    public GameObject IceBallScrap5;//脄采疭


    public Vector3 Force;

    private void Update()
    {
        if (gameObject.transform.position.y <= -500)
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
        IceBallScrap4.SetActive(false);
        IceBallScrap5.SetActive(false);

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
        while (isExploded && nowTime <= LiveTime)
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
        IceBallScrap4.SetActive(true);
        IceBallScrap5.SetActive(true);

        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    private void OnDisable()
    {
        isExploded = false;
        SpCollider.radius = 0.5f;
        pool.Return(gameObject);
    }
}
