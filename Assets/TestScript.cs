using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    [SerializeField] private Button trueButton1;
    [SerializeField] private Button trueButton2;
    [SerializeField] private Button falseButton1;
    [SerializeField] private Button falseButton2;

    private readonly BoolReactiveProperty _property1 = new();
    private readonly BoolReactiveProperty _property2 = new();
    
    private void Awake()
    {
        trueButton1.onClick.AddListener(() => _property1.Value = true);
        falseButton1.onClick.AddListener(() => _property1.Value = false);
        trueButton2.onClick.AddListener(() => _property2.Value = true);
        falseButton2.onClick.AddListener(() => _property2.Value = false);

        _property1.Where(v => v).Subscribe(_ =>
        {
            if (_property2.Value)
            {
                Debug.Log(":)");
            }
        });
    }
}
