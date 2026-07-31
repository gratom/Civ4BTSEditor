using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class XMLDoc<T> : ScriptableObject
{
    [SerializeField] public XMLDatabaseConfig xmlTextAsset;
    [SerializeField] public T value;
    
    public virtual void Object2XML()
    {
        if (xmlTextAsset != null)
        {
            string obj = SerializeObject(value);
            xmlTextAsset.SetText(obj);
            Debug.Log("xml " + xmlTextAsset.name + " saved");
        }
    }

    public virtual void XML2Object()
    {
        if (xmlTextAsset != null)
        {
            string xmlPart = xmlTextAsset.GetText();
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