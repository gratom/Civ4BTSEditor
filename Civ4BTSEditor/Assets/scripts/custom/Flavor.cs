using System;
using System.Xml.Serialization;

[Serializable]
public class Flavor
{
    [XmlElement("FlavorType")]
    public string FlavorType;

    [XmlElement("iFlavor")]
    public int flavor;
}