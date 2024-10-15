using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;  // 追蹤的目標（通常是 NPC）
    public float distance = 10.0f;  // 與目標的初始距離
    public float zoomSpeed = 2.0f;  // 縮放速度
    public float minDistance = 2.0f;  // 最小距離
    public float maxDistance = 20.0f; // 最大距離
    public float rotationSpeed = 5.0f;  // 滑鼠旋轉速度

    CameraCitizenTracker cameraCitizenTracker;

    private float currentX = 0.0f;
    private float currentY = 0.0f;

    private void Start()
    {
        cameraCitizenTracker = GetComponent<CameraCitizenTracker>();
    }

    void Update()
    {
        // 滑鼠右鍵旋轉
        if (Input.GetMouseButton(1))
        {
            currentX += Input.GetAxis("Mouse X") * rotationSpeed;
            currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            currentY = Mathf.Clamp(currentY, -90, 90);  // 限制Y軸旋轉角度
        }

        // 滑鼠滾輪縮放
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);



        target = cameraCitizenTracker.nowTrackingNPC;
    }

    void LateUpdate()
    {
        // 計算相機位置
        if (target != null)
        {
            Vector3 direction = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            transform.position = target.position + rotation * direction;
            transform.LookAt(target.position);
        }
    }
}
