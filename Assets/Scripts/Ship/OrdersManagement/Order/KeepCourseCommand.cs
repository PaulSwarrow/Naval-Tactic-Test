using Ship.AI.Data;
using Ship.Data;
using Ship.OrdersManagement;
using UnityEngine;

namespace Ship.AI.Order
{
    public class KeepCourseCommand : IShipOrder
    {
        private readonly float _course;


        public KeepCourseCommand(float course)
        {
            _course = course;
        }

        public ShipOrderCategory Category => ShipOrderCategory.Steer;

        public bool Execute(ShipBody ship)
        {
            var deltaAngle = _course - ship.Rotation.eulerAngles.y;
            deltaAngle *= 2f;//rotation effectiveness?
            deltaAngle = Mathf.Clamp(deltaAngle, -45, 45);
            
            ship.TurnWheel(deltaAngle);
            return false;
        }

        public bool Simulate(ManeuverContext context, float deltaTime)
        {
            var deltaAngle = _course - context.Ship.PhysicsData.Rotation.eulerAngles.y;
            deltaAngle *= 2f;//rotation effectiveness?
            deltaAngle = Mathf.Clamp(deltaAngle, -45, 45);

            //TODO simulate update
            context.Ship.Configuration.Steering.Angle = (int)deltaAngle;
            return false;
        }

        public void ApplyTo(ref ShipConfiguration configuration)
        {
            throw new System.NotImplementedException();
        }

        public ShipCommandEstimation Estimate(ShipConfiguration configuration)
        {
            return new ShipCommandEstimation()
            {
                Seconds = 1,
                CrewUnits = 1
            };
        }
    }
}