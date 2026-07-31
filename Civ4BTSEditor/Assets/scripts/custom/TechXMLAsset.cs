using UnityEngine;

[CreateAssetMenu(fileName = "TechAsset", menuName = "Civ4/Tech Asset")]
public class TechXMLAsset : XMLDoc<Civ4TechInfos>
{
    [ContextMenu("Object to XML")]
    public override void Object2XML()
    {
        if (xmlTextAsset != null)
        {
            string obj = SerializeObject(value);
            obj = obj.Substring(obj.IndexOf("<TechInfos>"));
            obj = obj.Substring(0, obj.IndexOf("</TechInfos>") + 12);
            xmlTextAsset.SetText(obj);
            Debug.Log("xml " + xmlTextAsset.name + " saved");
        }
    }
    
    [ContextMenu("XML to object")]
    public override void XML2Object()
    {
        if (xmlTextAsset != null)
        {
            string xmlPart = "<Civ4TechInfos xmlns=\"x-schema:CIV4TechnologiesSchema.xml\">\n" + 
                             xmlTextAsset.GetText() + 
                             "\n</Civ4TechInfos>";
            value = DeserializeObject<Civ4TechInfos>(xmlPart);
            Debug.Log("xml " + xmlTextAsset.name + " loaded");
        }
        else
        {
            Debug.Log("xml is null");
        }
    }
}