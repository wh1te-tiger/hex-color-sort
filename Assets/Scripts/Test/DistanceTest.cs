using System;
using DG.Tweening;
using UnityEngine;

public class DistanceTest : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform finish;
    [SerializeField] private Transform obj;
    [SerializeField] private float speed;

    private Sequence _sequence;
    private float Distance => Vector3.Distance(start.position + Vector3.up, finish.position+Vector3.up);
    private float Time => Distance / speed;
    
    private void OnValidate()
    {
       _sequence?.Kill();
       _sequence = DOTween.Sequence();
       obj.position = start.position + Vector3.up;
       _sequence.Append(obj.DOMove(finish.position + Vector3.up, Time)).SetLoops(-1, LoopType.Yoyo);
    }
}
