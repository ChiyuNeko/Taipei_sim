using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "hug-IceBall" || collision.gameObject.tag == "AccidentAlarm")
        {
            return;
        }
        else
        {
            Debug.Log($"destroy > {collision.gameObject.tag}");
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "hug-IceBall" || other.gameObject.tag == "AccidentAlarm")
        {
            return;
        }
        else
        {
            Debug.Log($"destroy > {other.gameObject.tag}");
            Destroy(other.gameObject);
        }
    }
}
