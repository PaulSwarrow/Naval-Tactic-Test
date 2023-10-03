using Ship.AI.Data;
using Ship.AI.Order;

namespace Ship.AI.Maneuvers
{
    public class TakeCourseManeuver : BaseShipManeuver
    {
        private float accuracy;
        private float targetAngle;

        public TakeCourseManeuver(float targetAngle, float accuracy  = 0.5f)
        {
            this.accuracy = accuracy;
            this.targetAngle = targetAngle;
        }

        protected override void DoCalculation(ManeuverContext context, ManeuverPrediction result)
        {
            CheckPoint(context, result, SteeringOrder.KeepCourse(targetAngle));
            
            //DO orders
            FastForward(3, context);
            CheckPoint(context, result);
            
            FastForward(3, context);
            CheckPoint(context, result);
            FastForward(3, context);
            CheckPoint(context, result);
            FastForward(3, context);
            CheckPoint(context, result);
            FastForward(3, context);
            CheckPoint(context, result);
            FastForward(3, context);
            CheckPoint(context, result);
            FastForward(3, context);
            CheckPoint(context, result);
            FastForward(3, context);
            CheckPoint(context, result);
            FastForward(3, context);
            CheckPoint(context, result);
            FastForward(3, context);
            CheckPoint(context, result);
            FastForward(3, context);
            CheckPoint(context, result);
            FastForward(3, context);
            CheckPoint(context, result);
            FastForward(3, context);
            CheckPoint(context, result);
            
            
        }
    }
}