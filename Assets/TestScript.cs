using System;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public GameObject[] objs;
    
    private Camera _camera;
    public float spacingFactor = 2;
    private void Awake()
    {
        _camera = Camera.main;
        var minX = float.MaxValue;
        var maxX = float.MinValue;
        foreach (var r in objs.Select(o=>o.GetComponent<Renderer>()))
        {
            var bounds = r.bounds;
            minX = Mathf.Min(bounds.min.x, minX);
            maxX = Mathf.Max(bounds.max.x, maxX);
        }
        
        Debug.Log($"min: {minX}, max: {maxX}");
        var orthographicSize = Mathf.Abs((maxX - minX)*spacingFactor) / _camera.aspect / 2f;
        _camera.orthographicSize = Mathf.Max(orthographicSize, .01f);
    }
    
    private float CalculateOrthographicSize(Bounds boundingBox)
    {
        var orthographicSize = _camera.orthographicSize;

        Vector2 min = boundingBox.min;
        Vector2 max = boundingBox.max;

        var width = (max - min).x * spacingFactor;
        var height = (max - min).y * spacingFactor;

        if (width > height)
        {
            orthographicSize = Mathf.Abs(width) / _camera.aspect / 2f;
        }
        else
        {
            orthographicSize = Mathf.Abs(height) / 2f;
        }

        return Mathf.Max(orthographicSize, .01f);
    }
}
