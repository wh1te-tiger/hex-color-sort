using TMPro;
using UnityEngine;

namespace Root
{
    public class EntityDebug : EntityProvider
    {
        [SerializeField] private TMP_Text text;
        protected override void Setup()
        {
            text.text = EntityId.ToString();
        }
    }
}