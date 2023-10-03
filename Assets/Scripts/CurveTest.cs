using System;
using System.Collections.Generic;
using Ship.Utils;
using UnityEngine;

namespace DefaultNamespace
{
    public class CurveTest : MonoBehaviour
    {
        [SerializeField]
        private int index;

        [SerializeField]
        private BezierCurveGenerator.Waypoint A;
        [SerializeField]
        private BezierCurveGenerator.Waypoint B;

        private List<Vector3> _curve;

        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, 500))
                {
                    Debug.Log(hit.transform.name);
                    Debug.Log($"SET {index}: {hit.point}");

                    CurrentPoint = new BezierCurveGenerator.Waypoint(hit.point, Vector3.forward);
                }
            }

            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, 500))
                {
                    var point = CurrentPoint;
                    point.Direction = (hit.point - point.Position).normalized*7;
                    Debug.Log($"SET DIRECTION {index}: {point.Direction}");
                    CurrentPoint = point;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                index = index == 0 ? 1 : 0;
                Debug.Log($"Now Choosing: {index}");

                if (A.Direction.magnitude * B.Direction.magnitude > 0)
                {
                    _curve = BezierCurveGenerator.Instance.GenerateBezierCurve(A, B);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (A.Direction.magnitude > 0)
            {
                DrawWaypoint(A, Color.green);
            }
            if (B.Direction.magnitude > 0)
            {
                DrawWaypoint(B, Color.blue);
            }

            if (_curve != null)
            {
                foreach (var waypoint in _curve)
                {
                    Gizmos.color = Color.yellow;
                    ;
                    Gizmos.DrawWireSphere(waypoint, 0.2f);
                }
            }
            
        }

        private void DrawWaypoint(BezierCurveGenerator.Waypoint waypoint, Color color, float r = 0.5f)
        {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(waypoint.Position, r);
            Gizmos.color = Color.white;
            Gizmos.DrawRay(waypoint.Position, waypoint.Direction);
        }

        private BezierCurveGenerator.Waypoint CurrentPoint
        {
            get => index == 0 ? A : B;
            set
            {
                if (index == 0) A = value;
                else B = value;
            }
        }
    }
}