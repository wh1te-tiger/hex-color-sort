using Scripts;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Field))]
    public class FieldEditor : UnityEditor.Editor
    {
        private Field _field;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _field = (Field)target;
            
            if (GUILayout.Button("Create Field"))
            {
                _field.CreateField();
            }
            
            if (GUILayout.Button("Save Field"))
            {
                if (!_field.HasSettings)
                {
                    var settings = CreateInstance<FieldSettings>();
                    AssetDatabase.CreateAsset(settings, "Assets/Configs/Game/Fields/Field..asset");
                    AssetDatabase.SaveAssets();
                    _field.FieldSettings = settings;
                }
                
                var serializedObject = new SerializedObject(_field.FieldSettings);
                EditorGUI.BeginChangeCheck();
                serializedObject.Update();
                
                _field.SaveField();
                
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(_field.FieldSettings);
            }

            GUI.enabled = _field.HasSettings;
            if (GUILayout.Button("Load Field"))
            {
                _field.LoadField();
            }

            GUI.enabled = true;
        }
    }
}