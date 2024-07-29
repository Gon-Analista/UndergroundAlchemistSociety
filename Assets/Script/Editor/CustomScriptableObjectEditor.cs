using UnityEngine;
using UnityEditor;
using BodyPart = Script.BodyParts.BodyPart;

namespace Script.Editor
{
    [CustomEditor(typeof(BodyPart))]
    public class CustomScriptableObjectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector
            DrawDefaultInspector();

            // Get a reference to the target scriptable object
            BodyPart scriptableObject = (BodyPart)target;

            // Set the id to the name of the file if it is null or empty
            if (string.IsNullOrEmpty(scriptableObject.id))
            {
                string path = AssetDatabase.GetAssetPath(scriptableObject);
                scriptableObject.id = System.IO.Path.GetFileNameWithoutExtension(path);
                EditorUtility.SetDirty(scriptableObject);
            }

            // Display the ID (read-only)
            EditorGUILayout.LabelField("ID", scriptableObject.id);
        }
    }
}