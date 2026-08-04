#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TechXMLAsset))]
public class TechXMLAssetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        try
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space(10);
            if (GUILayout.Button("Save to xml", GUILayout.Height(40)))
            {
                var techXMLAsset = (TechXMLAsset)target;
                if (techXMLAsset != null && techXMLAsset != null)
                {
                    Undo.RecordObject(techXMLAsset, "save to xml");
                    techXMLAsset.Object2XML();
                    EditorUtility.SetDirty(techXMLAsset);
                }
                else
                {
                    Debug.LogWarning("Non init");
                }
            }
        }
        catch (Exception e)
        {
            // Debug.LogError($"Error in techXML {e}");
        }
    }
}

#endif