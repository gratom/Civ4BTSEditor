using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Tools;
using UnityEditor;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Main : MonoBehaviour
{
    public static Main Instance => FindFirstObjectByType<Main>();

    public TechXMLAsset techAsset;

    public TechView techViewPrefab;

    [SerializeField] private List<TechView> techObjects;
    [SerializeField] private Transform parentTransform;

    public const string TXT_KEY = "TXT_KEY_";
    public const string DESCRIPTION = "";
    public const string CIVILOPEDIA = "_PEDIA";
    public const string STRATEGY = "_STRATEGY";
    public const string QUOTE = "_QUOTE";

    [FormerlySerializedAs("grapf")]
    public LineRenderer graf;
    public float scalerY;
    public Text grafPrefabTextCost;
    public List<Text> texts;


    public Dictionary<string, TechInfo> GetOrPrev(TechInfo tech)
    {
        Dictionary<string, TechInfo> ret = new Dictionary<string, TechInfo>();
        foreach (string preReq in tech.OrPreReqs)
        {
            TechInfo t = techAsset.value.TechInfos.FirstOrDefault(x => x.Type == preReq);
            if (t != null)
            {
                ret.Add(preReq, t);
            }
            else
            {
                Debug.Log(string.Format($"Tech for 'OR' {preReq} not found"));
            }
        }
        return ret;
    }

    public Dictionary<string, TechInfo> GetAndPrev(TechInfo tech)
    {
        Dictionary<string, TechInfo> ret = new Dictionary<string, TechInfo>();
        foreach (string preReq in tech.AndPreReqs)
        {
            TechInfo t = techAsset.value.TechInfos.FirstOrDefault(x => x.Type == preReq);
            if (t != null)
            {
                ret.Add(preReq, t);
            }
            else
            {
                Debug.Log(string.Format($"Tech for 'AND' {preReq} not found"));
            }
        }
        return ret;
    }

    [ContextMenu("Respawn objects from data")]
    public void Object2GameObjects()
    {
        DestroyOld();
        InitObjects();
    }

    [ContextMenu("Save to data from objects")]
    public void GameObjects2Object()
    {
        techObjects.RemoveAll(x => x == null);
        techAsset.value.TechInfos = new TechInfo[techObjects.Count];
        for (int i = 0; i < techObjects.Count; i++)
        {
            techAsset.value.TechInfos[i] = techObjects[i].data;
        }
    }

    [ContextMenu("Create new tech")]
    private void CreateNewTech()
    {
        TechView t = Instantiate(techViewPrefab, parentTransform);
        TechInfo data = techAsset.value.TechInfos[0].Copy();
        data.GridX = -1;
        data.Type += Random.Range(10000, 100000).ToString();
        data.Description = TXT_KEY + data.Type + DESCRIPTION;
        data.Civilopedia = TXT_KEY + data.Type + CIVILOPEDIA;
        data.Strategy = TXT_KEY + data.Type + STRATEGY;
        data.Quote = TXT_KEY + data.Type + QUOTE;
        data.Sound = "NONE";
        data.SoundMP = "NONE";

        t.InitWith(data, this);
        techObjects.Add(t);
        GameObjects2Object();
        Object2GameObjects();
    }

    public void CreateNewTech(TechInfo origin)
    {
        TechView t = Instantiate(techViewPrefab, parentTransform);
        TechInfo data = origin.Copy();
        data.GridX += 1;
        data.Type += Random.Range(10000, 100000).ToString();
        data.Description = TXT_KEY + data.Type + DESCRIPTION;
        data.Civilopedia = TXT_KEY + data.Type + CIVILOPEDIA;
        data.Strategy = TXT_KEY + data.Type + STRATEGY;
        data.Quote = TXT_KEY + data.Type + QUOTE;
        data.Sound = "NONE";
        data.SoundMP = "NONE";

        data.OrPreReqs.Clear();
        data.AndPreReqs.Clear();
        
        t.InitWith(data, this);
        techObjects.Add(t);
        GameObjects2Object();
        Object2GameObjects();
    }


    private void InitObjects()
    {
        HashSet<int> xPoses = new HashSet<int>();
        techObjects = new List<TechView>();
        for (int i = 0; i < techAsset.value.TechInfos.Length; i++)
        {
            TechView t = Instantiate(techViewPrefab, parentTransform);
            t.InitWith(techAsset.value.TechInfos[i], this);
            techObjects.Add(t);

            //graf
            xPoses.Add(techAsset.value.TechInfos[i].GridX);
        }


        texts = new List<Text>();
        List<Vector2> dotsForGraf = new List<Vector2>();
        foreach (int pos in xPoses)
        {
            int sum = techAsset.value.TechInfos.Where(x => x.GridX == pos).Sum(x => x.Cost);

            Text textInstance = Instantiate(grafPrefabTextCost, graf.transform, true);
            dotsForGraf.Add(new Vector2(TechView.CordToPos(pos, 0).x, sum));
            textInstance.transform.position = new Vector3(dotsForGraf.Last().x, dotsForGraf.Last().y * scalerY + 150, 0);
            textInstance.text = sum.ToString();
            texts.Add(textInstance);
        }
        dotsForGraf = dotsForGraf.OrderBy(x => x.x).ToList();
        graf.positionCount = dotsForGraf.Count;
        graf.SetPositions(dotsForGraf.Select(x => new Vector3(x.x, x.y * scalerY, 0)).ToArray());
        graf.SmoothBezier();
    }

    [MenuItem("Custom/refresh links")]
    public static void RefreshLinksFromMenu()
    {
        FindFirstObjectByType<Main>()?.RefreshLinks();
    }

    [MenuItem("Custom/Resresh All &r")]
    public static void RefreshAll()
    {
        Main m = Instance;
        if (m != null)
        {
            m.GameObjects2Object();
            m.Object2GameObjects();
            Debug.Log("Objects refreshed");
        }
    }

    [MenuItem("GameObject/Copy tech", false, 0)]
    private static void ContextSceneMenu(MenuCommand menuCommand)
    {
        
    }

    [ContextMenu("refresh links")]
    public void RefreshLinks()
    {
        for (int i = 0; i < techObjects.Count; i++)
        {
            techObjects[i].InitFromData();
        }
    }

    private void DestroyOld()
    {
        for (int i = 0; i < texts.Count; i++)
        {
            DestroyImmediate(texts[i].gameObject);
        }
        texts.Clear();
        if (techObjects != null)
        {
            for (int i = 0; i < techObjects.Count; i++)
            {
                TechView t = techObjects[i];
                DestroyImmediate(t.gameObject);
            }
        }
    }
}