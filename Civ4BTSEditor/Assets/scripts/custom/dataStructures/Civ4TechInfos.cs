using System;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("Civ4TechInfos", Namespace = "x-schema:CIV4TechnologiesSchema.xml")]
public class Civ4TechInfos
{
    [XmlArray("TechInfos")]
    [XmlArrayItem("TechInfo")]
    public TechInfo[] TechInfos;
}