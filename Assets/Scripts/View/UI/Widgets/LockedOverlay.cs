using System;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class LockedOverlay : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text value;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        public void Set(Vector3 screenPos, int v)
        {
            /*var rect = (RectTransform) transform;
            rect.anchoredPosition = screenPos;*/
            transform.position = screenPos;
            
            value.text = v.ToString();
        }

        public void Remove()
        {
            icon.transform.rotation = Quaternion.Euler(0,0,15);
            var sequence = DOTween.Sequence();
            sequence
                .Append(icon.transform.DORotate(new Vector3(0, 0, -30f), .2f, RotateMode.FastBeyond360)
                    .SetLoops(-1, LoopType.Yoyo).SetRelative())
                .Join(_canvasGroup.DOFade(0, 1.5f).SetDelay(1f))
                .Join(icon.transform.DOScale(Vector3.zero, .5f).SetDelay(1.5f));
        }
    }
}