using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hotkey : MonoBehaviour
{
    public GameObject[] GenerateObjectArray;
    //public Vector3 GenerateArea;
    //public Vector3 GenerateOffset;
    public float HightY;
    public Vector3 Force;


    public KeyCode Generate;
    public KeyCode MoveCamera;
    public KeyCode MoveCameraBack;

    public Transform RangeA;
    public Transform RangeB;

    void Start()
    {
    }
    void Update()
    {
        if(Input.GetKey(Generate))
        {
            DynamicGenerate(GenerateObjectArray[0], Force);
        }

        if(Input.GetKey(MoveCamera))
        {
            gameObject.GetComponent<DollyCameraControl>().MoveCamera();
        }

        if(Input.GetKey(MoveCameraBack))
        {
            gameObject.GetComponent<DollyCameraControl>().CameraBack();
        }
    }
    
    public void DynamicGenerate(GameObject gameObject, Vector3 addForce)
    {
        GameObject Generated = Instantiate(gameObject, new Vector3
            (Random.Range(RangeA.position.x, RangeB.position.x),
             HightY + Random.Range(-100, 100),
             Random.Range(RangeA.position.z, RangeB.position.z)), Quaternion.identity);

        //Generated.GetComponent<Rigidbody>().AddForce(addForce);
        Destroy(Generated, 5f); 
    }
}
