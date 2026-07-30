// using System.Collections.Generic;
// using Tools;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.UIElements;
// using Image = UnityEngine.UIElements.Image;
// public class TechObject : RectComponent
// {
//     public const float X_SCALING = 800;
//     public const float Y_SCALING = 150;
//
//     [SerializeField] public TechInfo data;
//     [SerializeField] private Image image;
//     [SerializeField] private Text textName;
//     [SerializeField] private Text textCost;
//
//     [SerializeField] private List<LineRendererWrapper> requireTechiesLines;
//     [SerializeField] private Main main;
//
//     [SerializeField] private Color techOrPre;
//     [SerializeField] private Color techAndPre;
//
//     [SerializeField] private LineRendererWrapper linePrefab;
//
//     [SerializeField] private List<CurveTool> curveTools;
//
//     public void InitWith(TechInfo data, Main initiator)
//     {
//         this.data = data;
//         main = initiator;
//         InitFromData();
//     }
//     private void MakeLines()
//     {
//         Dictionary<string, TechInfo> techOr = main.GetOrPrev(data);
//         foreach (string orPreReq in data.OrPreReqs)
//         {
//             requireTechiesLines.Add(CreateLineRenderer(techOrPre, techOr[orPreReq]));
//         }
//
//         Dictionary<string, TechInfo> techAnd = main.GetAndPrev(data);
//         foreach (string andPreReq in data.AndPreReqs)
//         {
//             requireTechiesLines.Add(CreateLineRenderer(techAndPre, techAnd[andPreReq]));
//         }
//     }
//     private LineRendererWrapper CreateLineRenderer(Color color, TechInfo tech)
//     {
//         LineRendererWrapper lineWrapper = Instantiate(linePrefab, gameObject.transform);
//         lineWrapper.color = color;
//         lineWrapper.SetDots(CordToPos(tech), CordToPos(data));
//         return lineWrapper;
//     }
//
//     private Vector2 CordToPos(TechInfo tech)
//     {
//         return new Vector2(tech.GridX * X_SCALING, -tech.GridY * Y_SCALING);
//     }
//
//     private void DestroyLines()
//     {
//         if (requireTechiesLines.Count > 0)
//         {
//             foreach (LineRendererWrapper line in requireTechiesLines)
//             {
//                 DestroyImmediate(line.gameObject);
//             }
//         }
//         requireTechiesLines = new List<LineRendererWrapper>();
//     }
//
//     [ContextMenu("Refresh")]
//     public void InitFromData()
//     {
//         if (data.Type.Length > 5)
//         {
//             textName.text = data.Type.Substring(5);
//         }
//         textCost.text = data?.Cost.ToString();
//         Position = CordToPos(data);
//         DestroyLines();
//         MakeLines();
//     }
//
//     private new void OnValidate()
//     {
//         base.OnValidate();
//         CheckPos();
//     }
//
//     private void OnDestroy()
//     {
//         Debug.Log("destr");
//     }
//
//     public void CheckPos()
//     {
//         int x = data.GridX;
//         int y = data.GridY;
//
//         data.GridX = Mathf.RoundToInt(Position.x / X_SCALING);
//         data.GridY = Mathf.RoundToInt(-Position.y / Y_SCALING);
//
//         if (x != data.GridX || y != data.GridY)
//         {
//             Debug.Log($"Pos updated {data.Type}");
//         }
//     }
// }