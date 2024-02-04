using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DefaultNamespace.GameSystems;
using Ship.AI;
using Ship.AI.Data;
using Ship.AI.Maneuvers;
using Ship.AI.Order;
using Ship.Data;
using Ship.OrdersManagement;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Ship
{
    public class ShipController : MonoBehaviour
    {
        [Inject] private WindSystem _windSystem;

        

        private ShipOrdersList _activeOrders = new ();
        private ShipBody _body;
        
        [SerializeField]
        private ManeuverPrediction _prediction;
        private void Awake()
        {
            _body = GetComponent<ShipBody>();
        }

        private void Start()
        {
            //TEST().Forget();
        }

        private void Update()
        {
            _activeOrders.ForeachOrder(order =>
            {
                if (order.Execute(_body)) _activeOrders.RemoveOrder(order);
                
            });
        }


        private async UniTask TEST()
        { 
            var context = new ManeuverContext()
            {
                Self = _body.ExportToAI(),
                Wind = _windSystem
            };
            var maneuver = new TakeCourseManeuver(Vector3.forward);
            _prediction = maneuver.Calculate(context);
            await ExecuteManeuver(_prediction);
            Debug.Log("Maneuver Complete");
        }

        private async UniTask ExecuteManeuver(ManeuverPrediction maneuver)
        {
            var time = 0f;
            foreach (var checkPoint in maneuver.Trajectory)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(checkPoint.Timestamp - time));
                time = checkPoint.Timestamp;
                //TODO check the deviation! Break if it's too large
                _activeOrders.AddRange(checkPoint.Orders);
                
            }
        }
        
        private void OnDrawGizmos()
        {
            if (_prediction == null) return;

            for (int i = 1; i < _prediction.Trajectory.Count; i++)
            {
                var prev = _prediction.Trajectory[i - 1];
                var cur = _prediction.Trajectory[i];
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(prev.Self.PhysicsData.Position, cur.Self.PhysicsData.Position);
                Gizmos.DrawWireSphere(cur.Self.PhysicsData.Position, 0.2f);
                Gizmos.color = Color.red;
                Gizmos.DrawRay(cur.Self.PhysicsData.Position, cur.Self.PhysicsData.Rotation * Vector3.forward);
            }
        }
        
    }
}