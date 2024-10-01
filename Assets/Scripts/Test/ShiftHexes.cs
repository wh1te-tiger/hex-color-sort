using System.Linq;
using DG.Tweening;
using Scripts;
using UnityEngine;

public class ShiftHexes : MonoBehaviour
{
    [SerializeField] private Transform cells;
    [SerializeField] private Transform hexes;
    [SerializeField] private ColorSettings colorSettings;
    [SerializeField] private ViewSettings viewSettings;
    [SerializeField] private float duration;
    [SerializeField] private float interval;

    private Transform _currentCell;
    private Sequence _sequence;
    private int _hexesCount;
    
    private void Start()
    {
        foreach (var col in cells.GetComponentsInChildren<Colorable>())
        {
            col.Color = viewSettings.BaseCellColor;
        }

        foreach (var col in hexes.GetComponentsInChildren<Colorable>())
        {
            col.Color = colorSettings.Get(ColorId.White);
        }

        _currentCell = cells.GetChild(0);
        _sequence = DOTween.Sequence();
        _hexesCount = hexes.childCount;
        SetNewTarget();
    }

    private void SetNewTarget()
    {
        _sequence.Kill();
        _sequence = DOTween.Sequence();
        var randomCell = PickCell();
        var targetPos = randomCell.transform.position;
        var pos = new Vector3(targetPos.x, 0, targetPos.z);
        var direction = (pos - _currentCell.transform.position).normalized;
        
        for (var i = _hexesCount - 1; i >= 0; i--)
        {
            var index = _hexesCount - 1 - i;
            var moving = hexes.GetChild(_hexesCount - 1);
            moving.SetSiblingIndex(index);
            Debug.Log($"Hex: {moving.name} now has sibling index: {index}");
            moving.rotation = Quaternion.LookRotation(direction, Vector3.up);
                
            /*var colorable = moving.GetComponent<Colorable>();
            var color = gradient[_gradientNumber % 2].Evaluate(index / (float) hexesCount);*/
                
            var height = (index + 1) * 0.3f;
                
            _sequence.Append(moving.DOLocalJump(new Vector3(pos.x, height, pos.z), 1, 1, duration));
            _sequence.Join(moving.DORotate(new Vector3(180, moving.rotation.eulerAngles.y, 0), duration));
            /*_sequence.Join(DOVirtual.Color(colorable.Color, color, duration,
                (v) => colorable.Color = v));*/
            _sequence.AppendCallback(() =>
            {
                moving.rotation = Quaternion.identity;
            });
        }
        
        Debug.Log("Sequence finished");
            
        _currentCell = randomCell;
        _sequence.AppendInterval(interval);
        _sequence.AppendCallback(SetNewTarget);
    }

    private Transform PickCell()
    {
        var transforms = cells.Cast<Transform>().Except(new[] { _currentCell }).ToArray();
        return transforms[Random.Range(0, transforms.Length)];
    }
}
