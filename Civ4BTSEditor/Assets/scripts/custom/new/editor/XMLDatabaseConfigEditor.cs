#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(XMLDatabaseConfig))]
public class XMLDatabaseConfigEditor : Editor
{
    private enum ConfirmAction
    {
        None,
        UpdateFromLink,
        SaveToLink
    }

    private ConfirmAction pendingAction = ConfirmAction.None;
    private double confirmationTimer = 0f;
    private const float ConfirmationDuration = 3f;

    private void OnEnable()
    {
        EditorApplication.update += OnEditorUpdate;
    }

    private void OnDisable()
    {
        EditorApplication.update -= OnEditorUpdate;
    }

    private void OnEditorUpdate()
    {
        if (pendingAction != ConfirmAction.None)
        {
            if (EditorApplication.timeSinceStartup > confirmationTimer)
            {
                pendingAction = ConfirmAction.None;
                Repaint();
            }
        }
    }

    public override void OnInspectorGUI()
    {
        XMLDatabaseConfig config = (XMLDatabaseConfig)target;

        DrawDefaultInspector();

        EditorGUILayout.Space(10);

        // Блок визуального превью отступов
        DrawXmlPreview(config);

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Выбрать внешний XML-файл...", GUILayout.Height(30)))
        {
            string initialDirectory = string.IsNullOrEmpty(config.externalFilePath)
                ? ""
                : System.IO.Path.GetDirectoryName(config.externalFilePath);

            string path = EditorUtility.OpenFilePanel("Выберите XML файл технологий Civ 4", initialDirectory, "xml");

            if (!string.IsNullOrEmpty(path))
            {
                config.externalFilePath = path;
                EditorUtility.SetDirty(config);
            }
        }

        EditorGUILayout.Space(15);

        #region button save to prj

        EditorGUILayout.BeginHorizontal();
        GUI.backgroundColor = new Color(0.6f, 1f, 0.6f);

        if (GUILayout.Button("Update from link -> local file\n(SAVE FILE TO UNITY)", GUILayout.Height(40)))
        {
            if (pendingAction != ConfirmAction.UpdateFromLink)
            {
                TriggerConfirmation(ConfirmAction.UpdateFromLink);
            }
        }

        if (pendingAction == ConfirmAction.UpdateFromLink)
        {
            GUI.backgroundColor = Color.green;
            double timeLeft = confirmationTimer - EditorApplication.timeSinceStartup;
            if (GUILayout.Button($"✔ YES?\n({timeLeft:F1}с)", GUILayout.Width(80), GUILayout.Height(40)))
            {
                config.UpdateFromExternalFile();
                ResetConfirmation();
            }
        }
        EditorGUILayout.EndHorizontal();

        #endregion

        EditorGUILayout.Space(5);

        #region button save to civ

        EditorGUILayout.BeginHorizontal();
        GUI.backgroundColor = new Color(1f, 0.7f, 0.5f);

        if (GUILayout.Button("Save local file -> link\n(SAVE FILE TO CIV)", GUILayout.Height(40)))
        {
            // Исправление бага из прошлого кода: клик по главной кнопке должен запускать подтверждение, а не проверяться на равенство
            if (pendingAction != ConfirmAction.SaveToLink)
            {
                TriggerConfirmation(ConfirmAction.SaveToLink);
            }
        }

        if (pendingAction == ConfirmAction.SaveToLink)
        {
            GUI.backgroundColor = new Color(1f, 0.4f, 0.4f);
            double timeLeft = confirmationTimer - EditorApplication.timeSinceStartup;
            if (GUILayout.Button($"✔ YES?\n({timeLeft:F1}с)", GUILayout.Width(80), GUILayout.Height(40)))
            {
                config.SaveToExternalFile();
                ResetConfirmation();
            }
        }
        EditorGUILayout.EndHorizontal();

        #endregion

        GUI.backgroundColor = Color.white;
    }

    private void DrawXmlPreview(XMLDatabaseConfig config)
    {
        EditorGUILayout.LabelField("XML Trim Preview", EditorStyles.boldLabel);

        if (config.internalXmlAsset == null)
        {
            EditorGUILayout.HelpBox("Внутренний XML-ассет не задан. Сделайте Update из файла мода.", MessageType.Info);
            return;
        }

        string fullText = config.internalXmlAsset.text;
        int length = fullText.Length;

        SerializedProperty pStartProp = serializedObject.FindProperty("prefixOffsetFromStart");
        SerializedProperty pEndProp = serializedObject.FindProperty("postfixOffsetFromEnd");

        int pStart = pStartProp != null ? Mathf.Clamp(pStartProp.intValue, 0, length) : 0;
        int pEnd = pEndProp != null ? Mathf.Clamp(pEndProp.intValue, 0, length - pStart) : 0;

        string prefixText = length > 0 ? fullText.Substring(0, Mathf.Min(pStart, length)) : "";
        string postfixText = length > pEnd ? fullText.Substring(length - pEnd) : "";

        string afterPrefixText = fullText.Substring(prefixText.Length, 50);
        string beforePostfixText = fullText.Substring(length - pEnd - 50, 50);
        
        GUIStyle boxStyle = new GUIStyle(EditorStyles.textArea);
        boxStyle.richText = true;
        boxStyle.fontSize = 10;
        boxStyle.wordWrap = true;

        string previewHtml = 
            $"<b><color=#E6DB74>{prefixText}</color>" +
            $"<b><color=#66D9EF>{afterPrefixText}...\n...{beforePostfixText}</color>" +
            $"<b><color=#FD971F>{postfixText}</color>";

        EditorGUILayout.LabelField(new GUIContent(previewHtml), boxStyle, GUILayout.MinHeight(250));
    }

    private void TriggerConfirmation(ConfirmAction action)
    {
        pendingAction = action;
        confirmationTimer = EditorApplication.timeSinceStartup + ConfirmationDuration;
        Repaint();
    }

    private void ResetConfirmation()
    {
        pendingAction = ConfirmAction.None;
        Repaint();
    }
}

#endif