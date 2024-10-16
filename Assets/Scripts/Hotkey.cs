using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hotkey : MonoBehaviour
{
    public GameObject[] GenerateObjectArray;
    public Vector3 GenerateArea;
    public Vector3 GenerateOffset;
    public Vector3 Force;


    public KeyCode Generate;
    public KeyCode MoveCamera;
    public KeyCode MoveCameraBack;
    


    void Start()
    {
    }
    void Update()
    {
        if(Input.GetKey(Generate))
        {
            DynamicGenerate(GenerateObjectArray[0], GenerateArea, GenerateOffset, Force);
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
    
    public void DynamicGenerate(GameObject gameObject, Vector3 GeneratePosition, Vector3 RandomOffset, Vector3 addForce)
    {
        GameObject Generated = Instantiate(gameObject, new Vector3
            (GeneratePosition.x + Random.Range(-RandomOffset.x,RandomOffset.x),
            GeneratePosition.y + Random.Range(-RandomOffset.y,RandomOffset.y),
            GeneratePosition.z + Random.Range(-RandomOffset.z,RandomOffset.z)), Quaternion.identity);

        Generated.GetComponent<Rigidbody>().AddForce(addForce);
        Destroy(Generated, 5f); 
    }
}
