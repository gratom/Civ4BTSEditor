#if UNITY_EDITOR
using System;
using UnityEditor;

[CustomEditor(typeof(TechXMLAsset))]
public class TechXMLAssetEditor : Editor
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