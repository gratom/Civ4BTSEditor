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
        }
        catch (Exception e)
        {
            // Debug.LogError(e);
        }
    }
}
#endif