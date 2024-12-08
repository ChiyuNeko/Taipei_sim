using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        Destroy(collision.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        Destroy(other.gameObject);
    }
}
