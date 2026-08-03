#if UNITY_EDITOR
using System;
using UnityEditor;
[CustomEditor(typeof(Main))]
public class MainEditor : Editor
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