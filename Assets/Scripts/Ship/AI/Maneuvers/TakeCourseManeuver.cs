using Ship.AI.Data;
using Ship.AI.Order;
using Ship.OrdersManagement;
using UnityEngine;

namespace Ship.AI.Maneuvers
{
    public class TakeCourseManeuver : BaseShipManeuver
    {
        private float _accuracy;
        private float _course;

        public TakeCourseManeuver(Vector3 direction, float accuracy  = 0.5f)
        {
            _course = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
            this._accuracy = accuracy;
        }
        public TakeCourseManeuver(float course, float accuracy  = 0.5f)
        {
            _course = course;
            this._accuracy = accuracy;
        }

        protected override void DoCalculation(ManeuverContext context, ManeuverPrediction result)
        {
            //CheckPoint(context, result, deltaAngle);
            TurnTo(_course, context, result);
            CheckPoint(context, result, ShipCommands.KeepCourse(_course));
            //DO orders
            FastForward(8, context);
            CheckPoint(context, result);
            
            
            
        }
    }
}