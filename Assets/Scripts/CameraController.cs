using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;  // �l�ܪ��ؼС]�q�`�O NPC�^
    public float distance = 10.0f;  // �P�ؼЪ���l�Z��
    public float zoomSpeed = 2.0f;  // �Y��t��
    public float minDistance = 2.0f;  // �̤p�Z��
    public float maxDistance = 20.0f; // �̤j�Z��
    public float rotationSpeed = 5.0f;  // �ƹ�����t��
    public Vector3 offset;

    CameraCitizenTracker cameraCitizenTracker;

    private float currentX = 0.0f;
    private float currentY = 0.0f;

    private void Start()
    {
        cameraCitizenTracker = GetComponent<CameraCitizenTracker>();
    }

    void Update()
    {
        // �ƹ��k�����
        if (Input.GetMouseButton(1))
        {
            currentX += Input.GetAxis("Mouse X") * rotationSpeed;
            currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            currentY = Mathf.Clamp(currentY, -90, 90);  // ����Y�b���ਤ��
        }

        // �ƹ��u���Y��
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);



        target = cameraCitizenTracker.nowTrackingNPC;
    }

    void LateUpdate()
    {
        // �p��۾���m
        if (target != null)
        {
            Vector3 direction = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            transform.position = target.position + rotation * direction + offset;
            transform.LookAt(target.position);
        }
    }
}
