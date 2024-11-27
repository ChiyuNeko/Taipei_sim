using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : MonoBehaviour
{
    private bool isExploded;

    private Rigidbody rb;

    public GameObject OriginalIceBall;//�Y���ɪ�����
    public GameObject IceBallScrap;//�z���ɤl�S��
    public GameObject AccidentAlarmArea;//Ĳ�o����isAccident���d��

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
