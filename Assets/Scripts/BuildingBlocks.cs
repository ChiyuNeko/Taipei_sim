using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlocks : MonoBehaviour
{
    public Vector3 Force;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //Force = new Vector3(1000,100000,1000);
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "hug-IceBall")
        {
            rb.AddForce(Force);
            //print("onHit");
        }
    }
}
