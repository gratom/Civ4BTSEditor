using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

[Serializable]
public class XMLDoc<T>
{
    [SerializeField] public TextAsset xmlTextAsset;
    [SerializeField] private string startWith;
    [SerializeField] private string cropThis;
    [SerializeField] public T value;

    public virtual void Object2XML()
    {
        if (xmlTextAsset != null)
        {
            string obj = SerializeObject(value);
            int index = obj.IndexOf(startWith, StringComparison.Ordinal);
            obj = obj.Substring(index);
            obj = cropThis + obj;

            string filePath = AssetDatabase.GetAssetPath(xmlTextAsset);

            StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8);

            obj = obj.Replace("&amp;", "&");

            writer.WriteLine(obj);
            writer.Close();

            File.WriteAllText(filePath, obj);
            AssetDatabase.Refresh();
            Debug.Log("xml " + xmlTextAsset.name + " saved");
        }
    }

    public virtual void XML2Object()
    {
        if (xmlTextAsset != null)
        {
            int index = xmlTextAsset.text.IndexOf(startWith, StringComparison.Ordinal);
            cropThis = xmlTextAsset.text.Substring(0, index);
            Debug.Log(cropThis);
            string xmlPart = xmlTextAsset.text.Substring(index);
            value = DeserializeObject<T>(xmlPart);
            Debug.Log("xml " + xmlTextAsset.name + " loaded");
        }
        else
        {
            Debug.Log("xml is null");
        }
    }

    protected static T2 DeserializeObject<T2>(string xmlString)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T2));
        StringReader stringReader = new StringReader(xmlString);
        return (T2)xmlSerializer.Deserialize(stringReader);
    }

    protected static string SerializeObject<T2>(T2 obj)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        StringWriter stringWriter = new StringWriter();
        xmlSerializer.Serialize(stringWriter, obj);
        return stringWriter.ToString();
    }

}