using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hugIceBallDropper : MonoBehaviour
{
    //public DisasterManager _dm;

    public int Scal;
    public DisasterFactorySO Pool;

    private void Start()
    {
        Pool.prewarm(Scal);
    }

    public void OnDisaster_hugIceBall(Transform dropPosistion)
    {
        GameObject dropIceBall = Pool.Request();
        dropIceBall.transform.position = dropPosistion.position;
    }
}
