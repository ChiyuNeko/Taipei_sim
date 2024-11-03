using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class DynamicNavMeshUpdater : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;  // Reference to the NavMeshSurface component
    public float updateInterval = 5.0f;    // Time interval for updating the NavMesh

    private bool isUpdating = false;       // Control whether the NavMesh is being updated

    void Start()
    {
        // 檢查是否有NavMeshSurface並開始動態更新
        if (navMeshSurface == null)
        {
            navMeshSurface = GetComponent<NavMeshSurface>();
        }

        if (navMeshSurface != null)
        {
            StartCoroutine(UpdateNavMesh());
        }
    }

    void Update()
    {
        // 如果你想手動觸發烘焙更新，也可以使用按鍵
        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(BakeNavMeshNow());
        }
    }

    // Coroutine to update the NavMesh at regular intervals
    IEnumerator UpdateNavMesh()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateInterval);
            StartCoroutine(BakeNavMeshNow());
        }
    }

    // Method to bake the NavMesh now
    IEnumerator BakeNavMeshNow()
    {
        if (isUpdating) yield break;  // 防止同時進行多次更新
        isUpdating = true;

        Debug.Log("Baking NavMesh...");

        // 更新NavMesh
        navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);

        yield return null;
        isUpdating = false;

        Debug.Log("NavMesh updated!");
    }
}
