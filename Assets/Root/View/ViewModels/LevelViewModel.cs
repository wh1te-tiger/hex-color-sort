using System.Linq;
using UniRx;
using UnityEngine;

namespace Root
{
    public class LevelViewModel : MonoBehaviour
    {
        [SerializeField] private Transform fieldRoot;
        [field: SerializeField] public Transform SlotsRoot { get; private set; }

        private void Awake()
        {
            SlotsRoot
                .GetComponentsInChildren<SlotViewModel>()
                .Select(view => view.ObserveEveryValueChanged(v => v.IsEmpty))
                .CombineLatest()
                .Select(values => values.All(v => v))
                .Where(v => v)
                .AsUnitObservable()
                .Subscribe(_ => HandleEmptySlots())
                .AddTo(this);
        }

        private void HandleEmptySlots()
        {
            Debug.Log("AllIsEmpty");
        }
    }
}
