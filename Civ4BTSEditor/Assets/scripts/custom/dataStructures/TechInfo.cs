using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
public class TechInfo
{
    [XmlElement("Type")]
    public string Type;

    [XmlElement("Description")]
    public string Description;

    [XmlElement("Civilopedia")]
    public string Civilopedia;

    [XmlElement("Help")]
    public string Help;

    [XmlElement("Strategy")]
    public string Strategy;

    [XmlElement("Advisor")]
    public string Advisor;

    [XmlElement("iAIWeight")]
    public int AILWeight;

    [XmlElement("iAITradeModifier")]
    public int AITradeModifier;

    [XmlElement("iCost")]
    public int Cost;

    [XmlElement("iAdvancedStartCost")]
    public int AdvancedStartCost;

    [XmlElement("iAdvancedStartCostIncrease")]
    public int AdvancedStartCostIncrease;

    [XmlElement("Era")]
    public string Era;

    [XmlElement("FirstFreeUnitClass")]
    public string FirstFreeUnitClass;

    [XmlElement("iFeatureProductionModifier")]
    public int FeatureProductionModifier;

    [XmlElement("iWorkerSpeedModifier")]
    public int WorkerSpeedModifier;

    [XmlElement("iTradeRoutes")]
    public int TradeRoutes;

    [XmlElement("iHealth")]
    public int Health;

    [XmlElement("iHappiness")]
    public int Happiness;

    [XmlElement("iFirstFreeTechs")]
    public int FirstFreeTechs;

    [XmlElement("iAsset")]
    public int Asset;

    [XmlElement("iPower")]
    public int Power;

    [XmlElement("bRepeat")]
    public int Repeat;

    [XmlElement("bTrade")]
    public int Trade;

    [XmlElement("bDisable")]
    public int Disable;

    [XmlElement("bGoodyTech")]
    public int GoodyTech;

    [XmlElement("bExtraWaterSeeFrom")]
    public int ExtraWaterSeeFrom;

    [XmlElement("bMapCentering")]
    public int MapCentering;

    [XmlElement("bMapVisible")]
    public int MapVisible;

    [XmlElement("bMapTrading")]
    public int MapTrading;

    [XmlElement("bTechTrading")]
    public int TechTrading;

    [XmlElement("bGoldTrading")]
    public int GoldTrading;

    [XmlElement("bOpenBordersTrading")]
    public int OpenBordersTrading;

    [XmlElement("bDefensivePactTrading")]
    public int DefensivePactTrading;

    [XmlElement("bPermanentAllianceTrading")]
    public int PermanentAllianceTrading;

    [XmlElement("bVassalTrading")]
    public int VassalTrading;

    [XmlElement("bBridgeBuilding")]
    public int BridgeBuilding;

    [XmlElement("bIrrigation")]
    public int Irrigation;

    [XmlElement("bIgnoreIrrigation")]
    public int IgnoreIrrigation;

    [XmlElement("bWaterWork")]
    public int WaterWork;

    [XmlElement("iGridX")]
    public int GridX;

    [XmlElement("iGridY")]
    public int GridY;

    [XmlArray("DomainExtraMoves")]
    [XmlArrayItem("DomainExtraMove")]
    public DomainExtraMove[] DomainExtraMoves;

    [XmlArray("CommerceFlexible")]
    [XmlArrayItem("bFlexible")]
    public List<int> CommerceFlexible;

    [XmlArray("TerrainTrades")]
    [XmlArrayItem("TerrainTrade")]
    public TerrainTrade[] TerrainTrades;

    [XmlElement("bRiverTrade")]
    public int RiverTrade;

    [XmlArray("Flavors")]
    [XmlArrayItem("Flavor")]
    public Flavor[] Flavors;

    [XmlArray("OrPreReqs")]
    [XmlArrayItem("PrereqTech")]
    public List<string> OrPreReqs;

    [XmlArray("AndPreReqs")]
    [XmlArrayItem("PrereqTech")]
    public List<string> AndPreReqs;

    [XmlElement("Quote")]
    public string Quote;

    [XmlElement("Sound")]
    public string Sound;

    [XmlElement("SoundMP")]
    public string SoundMP;

    [XmlElement("Button")]
    public string Button;

    // public void AndPreq(TechObject connectToTech)
    // {
    //     if (AndPreReqs.Contains(connectToTech.data.Type))
    //     {
    //         AndPreReqs.Remove(connectToTech.data.Type);
    //     }
    //     else
    //     {
    //         AndPreReqs.Add(connectToTech.data.Type);
    //     }
    // }
    // public void OrPreq(TechObject connectToTech)
    // {
    //     if (OrPreReqs.Contains(connectToTech.data.Type))
    //     {
    //         OrPreReqs.Remove(connectToTech.data.Type);
    //     }
    //     else
    //     {
    //         OrPreReqs.Add(connectToTech.data.Type);
    //     }
    // }
}