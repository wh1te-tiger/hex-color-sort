﻿using Data;
using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Field.", menuName = "Game Settings/FieldData", order = 0)]
    public class FieldSettings : ScriptableObject
    {
        [field: SerializeField] public CellData[] cells { get;  set; }
        [SerializeField] public Vector2Int[] coordinates;
    }
}