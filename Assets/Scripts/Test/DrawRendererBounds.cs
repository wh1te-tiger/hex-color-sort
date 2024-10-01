using UnityEngine;

public class DrawRendererBounds : MonoBehaviour
{
    public void OnDrawGizmosSelected()
    {
        var r = GetComponent<Renderer>();
        if (r == null)
            return;
        var bounds = r.bounds;
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.blue;
        Debug.Log(bounds.center);
        Debug.Log(bounds.extents);
        Gizmos.DrawWireCube(bounds.center, bounds.extents * 2);
    }
}
