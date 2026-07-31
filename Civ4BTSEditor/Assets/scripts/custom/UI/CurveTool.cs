using Tools;
using UnityEngine;

public class CurveTool : RectComponent
{
    public enum CurveType
    {
        and,
        or,
        deleteAnd,
        deleteOr
    }

    public TechView parent;
    public CurveType type;

}