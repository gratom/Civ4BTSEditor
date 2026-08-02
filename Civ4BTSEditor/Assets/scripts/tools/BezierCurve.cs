using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public static class BezierCurve
    {
        public static void SmoothBezier(this LineRenderer line, float smoothValue = 10)
        {
            if (line == null || line.positionCount < 3)
            {
                return;
            }

            List<Vector3> points = new List<Vector3>(line.positionCount);
            for (int i = 0; i < line.positionCount; i++)
            {
                points.Add(line.GetPosition(i));
            }

            List<Vector3> smoothedPoints = GetSmoothBezierSegments(points, Mathf.RoundToInt(smoothValue));

            line.positionCount = smoothedPoints.Count;
            line.SetPositions(smoothedPoints.ToArray());
        }

        private static List<Vector3> GetSmoothBezierSegments(List<Vector3> points, int resolution)
        {
            List<Vector3> curvePoints = new List<Vector3>();
            int count = points.Count;

            for (int i = 0; i < count - 1; i++)
            {
                Vector3 p0 = i == 0 ? points[0] : points[i - 1];
                Vector3 p1 = points[i];
                Vector3 p2 = points[i + 1];
                Vector3 p3 = i + 2 < count ? points[i + 2] : p2;

                Vector3 control1 = p1 + (p2 - p0) / 6f;
                Vector3 control2 = p2 - (p3 - p1) / 6f;

                int steps = i == count - 2 ? resolution : resolution - 1;

                for (int j = 0; j <= steps; j++)
                {
                    float t = j / (float)resolution;
                    curvePoints.Add(CalculateCubicBezierPoint(p1, control1, control2, p2, t));
                }
            }

            return curvePoints;
        }

        private static Vector3 CalculateCubicBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float u = 1f - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 p = uuu * p0;
            p += 3f * uu * t * p1;
            p += 3f * u * tt * p2;
            p += ttt * p3;

            return p;
        }

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