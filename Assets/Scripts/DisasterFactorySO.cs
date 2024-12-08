using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DisasterFactorySO : ScriptableObject
{
    public GameObject ObjectPrefab;
    public List<GameObject> ObjectPool;

    private int index = 0;
    private int listCount;

    public void prewarm(int scal)
    {
        ObjectPool.Clear();
        GameObject ObjectCollition = new GameObject();
        ObjectCollition.name = "ObjectCollition";

        for (int i = 0; i < scal; i++)
        {
            GameObject newDisasterObject = Instantiate(ObjectPrefab, ObjectCollition.transform);
            ObjectPool.Add(newDisasterObject);
            newDisasterObject.SetActive(false);
        }
        listCount = scal;
    }

    public GameObject Request()
    {
        if (ObjectPool.Count <= 0)
        {
            Debug.LogError("¼Æ¶q¤£¨¬");
            return null;
        }
        else
        {
            GameObject requestDisasterObject = ObjectPool[index % listCount];
            requestDisasterObject.SetActive(true);
            index++;
            return requestDisasterObject;
        }
    }

    public void Return(GameObject returnDisasterObject)
    {
        returnDisasterObject.SetActive(false);
    }
}
