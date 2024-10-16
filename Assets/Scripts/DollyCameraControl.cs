using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DollyCameraControl : MonoBehaviour
{
    public CinemachineVirtualCamera currentCamera;
    public CinemachineTrackedDolly cinemachineTrackedDolly;
    public float Speed;
    public float position;

    void Start()
    {
        cinemachineTrackedDolly = currentCamera.GetCinemachineComponent<CinemachineTrackedDolly> ();

    }
    

    public void MoveCamera()
    {
        position += Speed * Time.deltaTime;
        cinemachineTrackedDolly.m_PathPosition = position;
    }
    public void CameraBack()
    {
        position -= Speed * Time.deltaTime;
        cinemachineTrackedDolly.m_PathPosition = position;
    }
}
