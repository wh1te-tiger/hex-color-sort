using System;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image fill;
        [SerializeField] private float fillDuration;
        [SerializeField] private TMP_Text progressText;
        [SerializeField] private ProgressDisplayType displayType;

        private int _maxValue;
        private bool _hasMaxValue;
        private readonly StringBuilder _stringBuilder = new();
        
        public void SetBounds(int maxValue)
        {
            _maxValue = maxValue;
            _hasMaxValue = true;
            UpdateValues(0, forced: true);
        }

        public void SetFillAmountUnclamped(int value)
        {
            if (!_hasMaxValue) throw new Exception("Max value is not set");
            
            UpdateValues(value);
        }
        
        private void UpdateValues(int value, bool forced = false)
        {
            var clampedValue = value / (float) _maxValue;
            fill.DOFillAmount(clampedValue, forced? 0f : fillDuration);

            _stringBuilder.Clear();
            switch (displayType)
            {
                case ProgressDisplayType.Percentage:
                    _stringBuilder.AppendFormat("{0:P}", clampedValue);
                    break;
                case ProgressDisplayType.Bounds:
                    if(!_hasMaxValue) throw new Exception("Max value is not set");
                    _stringBuilder.Append($"{Mathf.CeilToInt(value)} / {_maxValue}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            progressText.text = _stringBuilder.ToString();
        }
    }

    public enum ProgressDisplayType
    {
        Percentage,
        Bounds
    }
}