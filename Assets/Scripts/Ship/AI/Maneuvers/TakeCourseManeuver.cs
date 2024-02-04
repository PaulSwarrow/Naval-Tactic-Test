using Ship.AI.Data;
using Ship.AI.Order;
using Ship.OrdersManagement;
using UnityEngine;

namespace Ship.AI.Maneuvers
{
    public class TakeCourseManeuver : BaseShipManeuver
    {
        private float _accuracy;
        private Vector3 _direction;

        public TakeCourseManeuver(Vector3 direction, float accuracy  = 0.5f)
        {
            _direction = direction;
            this._accuracy = accuracy;
        }

        protected override void DoCalculation(ManeuverContext context, ManeuverPrediction result)
        {
            // CheckPoint(context, result, SteeringOrders.KeepCourse(targetAngle));
            var deltaAngle = Vector3.SignedAngle(context.Ship.PhysicsData.Forward, _direction, Vector3.up);
            //CheckPoint(context, result, deltaAngle);
            TurnTo(_direction, context, result);
            CheckPoint(context, result, ShipCommands.KeepCourse(_direction));
            //DO orders
            FastForward(8, context);
            CheckPoint(context, result);
            
            
            
        }
    }
}