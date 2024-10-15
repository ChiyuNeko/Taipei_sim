using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotkey : MonoBehaviour
{
    public GameObject[] GenerateObjectArray;
    public Vector3 GenerateArea;
    public Vector3 GenerateOffset;
    public Vector3 Force;

    
    void Update()
    {
        if(Input.GetKey(KeyCode.F))
        {
            DynamicGenerate(GenerateObjectArray[0], GenerateArea, GenerateOffset, Force);
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
