using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform target;
    public float height = 10f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 pos = target.position;
        pos.y += height;
        transform.position = pos;
    }
}