#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(XMLDatabaseConfig))]
public class XMLDatabaseConfigEditor : Editor
{
    // Перечисление для отслеживания, какое действие ожидает подтверждения
    private enum ConfirmAction
    {
        None,
        UpdateFromLink,
        SaveToLink
    }

    private ConfirmAction pendingAction = ConfirmAction.None;
    private double confirmationTimer = 0f;
    private const float ConfirmationDuration = 3f; // Время на подтверждение в секундах

    private void OnEnable()
    {
        // Подписываемся на обновление редактора для платного таймера сброса
        EditorApplication.update += OnEditorUpdate;
    }

    private void OnDisable()
    {
        EditorApplication.update -= OnEditorUpdate;
    }

    private void OnEditorUpdate()
    {
        // Если таймер активен, проверяем не истекло ли время
        if (pendingAction != ConfirmAction.None)
        {
            if (EditorApplication.timeSinceStartup > confirmationTimer)
            {
                pendingAction = ConfirmAction.None;
                Repaint(); // Перерисовываем инспектор, чтобы скрыть кнопку подтверждения
            }
        }
    }

    public override void OnInspectorGUI()
    {
        XMLDatabaseConfig config = (XMLDatabaseConfig)target;

        DrawDefaultInspector();

        EditorGUILayout.Space(10);

        // Кнопка выбора файла через проводник ОС
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

        // --- КНОПКА 1: Update from link -> local file ---
        EditorGUILayout.BeginHorizontal();
        GUI.backgroundColor = new Color(0.6f, 1f, 0.6f);
        
        if (GUILayout.Button("Update from link -> local file\n(SAVE FILE TO UNITY)", GUILayout.Height(40)))
        {
            if (pendingAction == ConfirmAction.UpdateFromLink)
            {
                // // Подтверждено — выполняем действие
                // config.UpdateFromExternalFile();
                // ResetConfirmation();
            }
            else
            {
                // Первый клик — запрашиваем подтверждение
                TriggerConfirmation(ConfirmAction.UpdateFromLink);
            }
        }

        // Если это действие ждет подтверждения, рисуем рядом кнопку-галочку
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

        EditorGUILayout.Space(5);

        // --- КНОПКА 2: Save local file -> link ---
        EditorGUILayout.BeginHorizontal();
        GUI.backgroundColor = new Color(1f, 0.7f, 0.5f);
        
        if (GUILayout.Button("Save local file -> link\n(SAVE FILE TO CIV)", GUILayout.Height(40)))
        {
            if (pendingAction == ConfirmAction.SaveToLink)
            {
                // // Подтверждено — выполняем действие
                // config.SaveToExternalFile();
                // ResetConfirmation();
            }
            else
            {
                // Первый клик — запрашиваем подтверждение
                TriggerConfirmation(ConfirmAction.SaveToLink);
            }
        }

        // Если это действие ждет подтверждения, рисуем рядом кнопку-галочку
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

        GUI.backgroundColor = Color.white;
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