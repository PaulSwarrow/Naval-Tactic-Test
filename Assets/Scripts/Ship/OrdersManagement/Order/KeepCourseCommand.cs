using Ship.AI.Data;
using Ship.Data;
using Ship.OrdersManagement;
using UnityEngine;

namespace Ship.AI.Order
{
    public class KeepCourseCommand : IShipOrder
    {
      
        private readonly Vector3 _targetDirection;

        public KeepCourseCommand(Vector3 targetDirection)
        {
            this._targetDirection = targetDirection;
        }

        public ShipOrderCategory Category => ShipOrderCategory.Steer;

        public bool Execute(ShipBody ship)
        {
            var forward = ship.Rotation * Vector3.forward;
            var direction = _targetDirection;
            var deltaAngle = Vector3.SignedAngle(forward, direction, Vector3.up);
            deltaAngle *= 2f;//rotation effectiveness?
            deltaAngle = Mathf.Clamp(deltaAngle, -45, 45);
            
            ship.TurnWheel(deltaAngle);
            return false;
        }

        public bool Simulate(ManeuverContext context, float deltaTime)
        {
            var forward = context.Ship.PhysicsData.Rotation * Vector3.forward;
            var direction = _targetDirection;
            var deltaAngle = Vector3.SignedAngle(forward, direction, Vector3.up);
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