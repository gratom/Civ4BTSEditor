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

    /// <summary>
    /// Импорт: копирует файл из внешней папки мода внутрь проекта Unity
    /// </summary>
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

        // Копируем файл физически (перезаписываем локальный)
        File.Copy(externalFilePath, internalAssetPath, true);

        // Обновляем базу данных Unity, чтобы она подхватила изменения
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

            // Создаем пустой XML-файл (или файл с базовой структурой), если его не было
            File.WriteAllText(internalAssetPath, "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<Civ4TechInfos xmlns=\"\">\n</Civ4TechInfos>");

            AssetDatabase.ImportAsset(internalAssetPath);
            internalXmlAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(internalAssetPath);
            EditorUtility.SetDirty(this);
        }

        string fullInternalPath = Path.GetFullPath(internalAssetPath);

        // Гарантируем, что внешняя папка назначения существует
        string externalDir = Path.GetDirectoryName(externalFilePath);
        if (!Directory.Exists(externalDir))
        {
            Directory.CreateDirectory(externalDir);
        }

        // Перезаписываем внешний файл мода
        File.Copy(fullInternalPath, externalFilePath, true);
        Debug.Log($"[TechConfig] Успешно сохранено во внешний файл: {externalFilePath}");
    }

    /// <summary>
    /// Создает путь для внутренней копии рядом с самим ScriptableObject
    /// </summary>
    private string GetOrCreateInternalAssetPath()
    {
        string soPath = AssetDatabase.GetAssetPath(this);
        if (string.IsNullOrEmpty(soPath))
        {
            return null;
        }

        string directory = Path.GetDirectoryName(soPath);

        // Файл будет называться так же, как SO, но с расширением .xml
        string fileName = $"{name}_Copy.xml";
        return Path.Combine(directory, fileName).Replace("\\", "/");
    }
}