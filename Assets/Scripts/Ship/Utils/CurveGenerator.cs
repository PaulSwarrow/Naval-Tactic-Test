using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ship.Utils
{
    public class BezierCurveGenerator
    {
        public static readonly BezierCurveGenerator Instance = new();
        public struct Waypoint
        {
            public Vector3 Position;
            public Vector3 Direction;

            public Waypoint(Vector3 position, Vector3 direction)
            {
                Position = position;
                Direction = direction.normalized; // Normalize the direction vector
            }
        }
        // Adjustable parameters
        public int NumSegments = 10; // Number of segments in the curve

        public List<Vector3> GenerateBezierCurve(Waypoint startPoint, Waypoint endPoint)
        {
            List<Vector3> curvePoints = new List<Vector3>();

            for (int i = 0; i <= NumSegments; i++)
            {
                float t = i / (float)NumSegments;

                Vector3 position = CalculateBezierPoint(startPoint.Position, startPoint.Direction, endPoint.Position, endPoint.Direction, t);
                curvePoints.Add(position);
            }

            return curvePoints;
        }

        private Vector3 CalculateBezierPoint(Vector3 p0, Vector3 d0, Vector3 p1, Vector3 d1, float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 p = uuu * p0; // (1-t)^3 * P0
            p += 3 * uu * t * (p0 + d0); // 3(1-t)^2 * t * (P0 + D0)
            p += 3 * u * tt * (p1 + d1); // 3(1-t) * t^2 * (P1 + D1)
            p += ttt * p1; // t^3 * P1

            return p;
        }
    }
}
