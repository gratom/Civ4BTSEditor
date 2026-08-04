#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TechView))]
public class TechViewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        try
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space(10);
            if (GUILayout.Button("Sync Button with name", GUILayout.Height(40)))
            {
                TechView techView = (TechView)target;
                if (techView != null && techView.data != null)
                {
                    Undo.RecordObject(techView, "Modify TechView Data");
                    PerformCustomAction(techView.data);
                    EditorUtility.SetDirty(techView);
                }
                else
                {
                    Debug.LogWarning("TechView not init");
                }
            }
        }
        catch (Exception e)
        {
            // Debug.LogError($"error {e}");
        }
    }

    private void PerformCustomAction(TechInfo data)
    {
        if (data.Type.Length >= 2)
        {
            string nm = data.Type.ToLower().Substring(5);
            data.Button = "Art/Interface/Buttons/TechTree/" + nm.Substring(0, 1).ToUpper() + nm.Substring(1) + ".dds";
        }
    }
}

#endif