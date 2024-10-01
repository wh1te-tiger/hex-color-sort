using System;
using Data;
using Scripts;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(FieldSettings))]
    public class FieldEditor : UnityEditor.Editor
    {
        private FieldSettings _settings;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Fill"))
            {
                _settings = (FieldSettings)target;
                _settings.cells = new CellData[_settings.coordinates.Length];
                for (var i = 0; i < _settings.coordinates.Length; i++)
                {
                    var cor = _settings.coordinates[i];
                    var cellData = new CellData
                    {
                        coordinates = new Coordinates(cor.x, cor.y),
                        hexes = Array.Empty<HexData>()
                    };
                    _settings.cells[i] = cellData;
                }
            }
        }
    }
}