using System;
using System.Xml.Serialization;

[Serializable]
public class TerrainTrade
{
    [XmlElement(ElementName = "TerrainType")]
    public string TerrainType;

    [XmlElement(ElementName = "bTerrainTrade")]
    public int BTerrainTrade;
}