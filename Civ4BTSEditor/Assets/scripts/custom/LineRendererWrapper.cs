using System.Collections.Generic;
using Tools;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class LineRendererWrapper : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [Range(0.01f, 0.99f)][SerializeField] private float smoothGlobal;
    [Range(2, 100)][SerializeField] private int smoothLocal;

    public Color color
    {
        get => line.startColor;
        set
        {
            line.startColor = value;
            line.endColor = value;
        }
    }

    public void SetDots(Vector2 start, Vector2 finish)
    {
        float xCenter = Mathf.Lerp(finish.x, start.x, 0.5f);
        List<Vector3> dots = new List<Vector3>()
        {
            start,
            new Vector2(Mathf.Lerp(start.x, xCenter, smoothGlobal), start.y),
            new Vector2(xCenter, start.y),
            new Vector2(xCenter, finish.y),
            new Vector2(Mathf.Lerp(finish.x, xCenter, smoothGlobal), finish.y),
            finish
        };
        List<Vector3> bezier = BezierCurve.Bezier(dots, smoothLocal);
        line.positionCount = bezier.Count;
        line.SetPositions(bezier.ToArray());
    }

    private void OnValidate()
    {
        if (line == null)
        {
            line = GetComponent<LineRenderer>();
        }
    }
}