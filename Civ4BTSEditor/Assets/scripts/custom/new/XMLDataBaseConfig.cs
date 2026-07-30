using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "XMLDatabaseConfig", menuName = "Civ4/XML Database Config")]
public class XMLDatabaseConfig : ScriptableObject
{
    [Header("External Path (Real file in your mod folder)")]
    [Tooltip("Абсолютный путь к оригинальному XML-файлу в директории твоего мода")]
    public string externalFilePath;

    [Header("Internal Unity Asset")]
    [Tooltip("Локальная копия файла внутри проекта Unity для работы парсера")]
    public TextAsset internalXmlAsset;

    public void UpdateFromExternalFile()
    {
        if (string.IsNullOrEmpty(externalFilePath) || !File.Exists(externalFilePath))
        {
            Debug.LogError($"[TechConfig] Внешний путь не задан или файл не существует: {externalFilePath}");
            return;
        }

        string internalAssetPath = GetOrCreateInternalAssetPath();
        if (string.IsNullOrEmpty(internalAssetPath))
        {
            return;
        }

        File.Copy(externalFilePath, internalAssetPath, true);

        AssetDatabase.ImportAsset(internalAssetPath);
        internalXmlAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(internalAssetPath);

        EditorUtility.SetDirty(this);
        Debug.Log($"[TechConfig] Успешно импортировано из: {externalFilePath}");
    }

    public void SaveToExternalFile()
    {
        if (string.IsNullOrEmpty(externalFilePath))
        {
            Debug.LogError("[TechConfig] Не задан внешний путь для сохранения!");
            return;
        }

        // Проверяем, существует ли внутренний ассет
        string internalAssetPath = internalXmlAsset != null ? AssetDatabase.GetAssetPath(internalXmlAsset) : string.Empty;

        if (string.IsNullOrEmpty(internalAssetPath) || !File.Exists(Path.GetFullPath(internalAssetPath)))
        {
            Debug.LogWarning("[TechConfig] Внутренний TextAsset не найден или отсутствует. Создаем новый локальный файл...");

            internalAssetPath = GetOrCreateInternalAssetPath();
            if (string.IsNullOrEmpty(internalAssetPath))
            {
                return;
            }
            File.WriteAllText(internalAssetPath, "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Civ4TechInfos xmlns=\"\">\n</Civ4TechInfos>");
            AssetDatabase.ImportAsset(internalAssetPath);
            internalXmlAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(internalAssetPath);
            EditorUtility.SetDirty(this);
        }

        string fullInternalPath = Path.GetFullPath(internalAssetPath);

        string externalDir = Path.GetDirectoryName(externalFilePath);
        if (!Directory.Exists(externalDir))
        {
            Directory.CreateDirectory(externalDir);
        }

        File.Copy(fullInternalPath, externalFilePath, true);
        Debug.Log($"[TechConfig] Успешно сохранено во внешний файл: {externalFilePath}");
    }

    private string GetOrCreateInternalAssetPath()
    {
        string soPath = AssetDatabase.GetAssetPath(this);
        if (string.IsNullOrEmpty(soPath))
        {
            return null;
        }

        string directory = Path.GetDirectoryName(soPath);
        string fileName = $"{name}_Copy.xml";
        return Path.Combine(directory, fileName).Replace("\\", "/");
    }
}