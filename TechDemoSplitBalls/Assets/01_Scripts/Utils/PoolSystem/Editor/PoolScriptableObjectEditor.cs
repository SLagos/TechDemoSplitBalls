using UnityEditor;
using UnityEngine;

namespace Utils.PoolSystem.Editor
{
    [CustomEditor(typeof(PoolScriptableObject))]
    public class PoolScriptableObjectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Save"))
            {
                AssetDatabase.Refresh();
            }
        }
    }
}