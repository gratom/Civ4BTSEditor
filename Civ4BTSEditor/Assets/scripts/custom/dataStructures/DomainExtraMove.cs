using System;
using System.Xml.Serialization;

[Serializable]
public class DomainExtraMove
{
    [XmlElement(ElementName = "DomainType")]
    public string DomainType;

    [XmlElement(ElementName = "iExtraMoves")]
    public int IExtraMoves;
}