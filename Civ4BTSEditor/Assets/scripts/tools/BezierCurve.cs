using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public static class BezierCurve
    {
        public static List<Vector3> Bezier(List<Vector3> dots, float smoothValue)
        {
            List<Vector3> curvePoints = new List<Vector3>();

            int numPoints = Mathf.RoundToInt(smoothValue);

            for (int i = 0; i <= numPoints; i++)
            {
                float t = i / (float)numPoints;
                curvePoints.Add(GetBezierPoint(dots, t));
            }

            return curvePoints;
        }

        private static Vector3 GetBezierPoint(List<Vector3> dots, float t)
        {
            if (dots.Count == 1)
            {
                return dots[0];
            }

            List<Vector3> tempDots = new List<Vector3>(dots);

            while (tempDots.Count > 1)
            {
                for (int i = 0; i < tempDots.Count - 1; i++)
                {
                    tempDots[i] = Vector3.Lerp(tempDots[i], tempDots[i + 1], t);
                }

                tempDots.RemoveAt(tempDots.Count - 1);
            }

            return tempDots[0];
        }
    }
}