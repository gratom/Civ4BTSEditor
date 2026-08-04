using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UIElements.Image;

public class TechView : RectComponent
{
    public const float X_SCALING = 800;
    public const float Y_SCALING = 150;
    
    [SerializeField] public TechInfo data;
    [SerializeField] private Image image;
    [SerializeField] private Text textName;
    [SerializeField] private Text textCost;

    [SerializeField] private List<LineRendererWrapper> requireTechiesLines;
    [SerializeField] private Main main;

    [SerializeField] private Color techOrPre;
    [SerializeField] private Color techAndPre;

    [SerializeField] private LineRendererWrapper linePrefab;

    public void InitWith(TechInfo data, Main initiator)
    {
        this.data = data;
        main = initiator;
        InitFromData();
    }

    private void MakeLines()
    {
        Dictionary<string, TechInfo> techOr = main.GetOrPrev(data);
        foreach (string orPreReq in data.OrPreReqs)
        {
            requireTechiesLines.Add(CreateLineRenderer(techOrPre, techOr[orPreReq]));
        }

        Dictionary<string, TechInfo> techAnd = main.GetAndPrev(data);
        foreach (string andPreReq in data.AndPreReqs)
        {
            requireTechiesLines.Add(CreateLineRenderer(techAndPre, techAnd[andPreReq]));
        }
    }

    private LineRendererWrapper CreateLineRenderer(Color color, TechInfo tech)
    {
        LineRendererWrapper lineWrapper = Instantiate(linePrefab, gameObject.transform);
        lineWrapper.color = color;
        lineWrapper.SetDots(CordToPos(tech), CordToPos(data));
        return lineWrapper;
    }

    public static Vector2 CordToPos(TechInfo tech)
    {
        return new Vector2(tech.GridX * X_SCALING, -tech.GridY * Y_SCALING);
    }

    public static Vector2 CordToPos(int gridX, int gridY)
    {
        return new Vector2(gridX * X_SCALING, -gridY * Y_SCALING);
    }

    private void DestroyLines()
    {
        if (requireTechiesLines.Count > 0)
        {
            foreach (LineRendererWrapper line in requireTechiesLines)
            {
                DestroyImmediate(line.gameObject);
            }
        }
        requireTechiesLines = new List<LineRendererWrapper>();
    }

    [ContextMenu("Refresh")]
    public void InitFromData()
    {
        if (data.Type.Length > 5)
        {
            textName.text = data.Type.Substring(5);
        }
        textCost.text = data?.Cost.ToString();
        AnchoredPosition = CordToPos(data);
        DestroyLines();
        MakeLines();
    }

    private new void OnValidate()
    {
        base.OnValidate();
        CheckPos();
        CheckNames();
    }

    private void CheckNames()
    {
        data.Description = Main.TXT_KEY + data.Type + Main.DESCRIPTION;
        data.Civilopedia = Main.TXT_KEY + data.Type + Main.CIVILOPEDIA;
        data.Strategy = Main.TXT_KEY + data.Type + Main.STRATEGY;
        data.Quote = Main.TXT_KEY + data.Type + Main.QUOTE;
        //data.Button = "Art/Interface/Buttons/TechTree/" + data.Type.Substring(0, 1) + data.Type.ToLower().Substring(1);
    }

    public void CheckPos()
    {
        if (data == null)
        {
            return;
        }

        int x = data.GridX;
        int y = data.GridY;

        data.GridX = Mathf.RoundToInt(AnchoredPosition.x / X_SCALING);
        data.GridY = Mathf.RoundToInt(-AnchoredPosition.y / Y_SCALING);

        if (x != data.GridX || y != data.GridY)
        {
            Debug.Log($"Pos updated {data.Type}");
            InitFromData();
        }
    }
}