using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterDropper : MonoBehaviour
{
    public int Scal;
    public DisasterFactorySO Pool;

    public float DropOffsetX;
    public float DropOffsetZ;

    private void Start()
    {
        Pool.prewarm(Scal);
    }

    public void OnDisaster(Vector3 dropPosistion)
    {
        float posX = dropPosistion.x + Random.Range(-DropOffsetX, DropOffsetX);
        float posY = dropPosistion.y;
        float posZ = dropPosistion.z + Random.Range(-DropOffsetZ, DropOffsetZ); ;

        GameObject DisasterObj = Pool.Request();
        DisasterObj.transform.position = new Vector3(posX, posY, posZ);
    }
}
