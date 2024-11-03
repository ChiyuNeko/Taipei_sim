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
        // �ˬd�O�_��NavMeshSurface�ö}�l�ʺA��s
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
        // �p�G�A�Q���Ĳ�o�M�H��s�A�]�i�H�ϥΫ���
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
        if (isUpdating) yield break;  // ����P�ɶi��h����s
        isUpdating = true;

        Debug.Log("Baking NavMesh...");

        // ��sNavMesh
        navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);

        yield return null;
        isUpdating = false;

        Debug.Log("NavMesh updated!");
    }
}
