using Ship.AI.Data;
using Ship.OrdersManagement;
using UnityEngine;

namespace Ship.AI.Order
{
    public class SteeringOrder : IShipOrder
    {
        public SteeringOrder(float targetDirection)
        {
            this._targetDirection = targetDirection;
        }

        public static IShipOrder KeepCourse(float direction) => new SteeringOrder(direction);
        

        private float _targetDirection;

        public ShipOrderCategory Category => ShipOrderCategory.Steer;

        public bool Execute(ShipBody ship)
        {
            var deltaAngle = _targetDirection - ship.Rotation.eulerAngles.y;
            deltaAngle *= 2f;//rotation effectiveness?
            deltaAngle = Mathf.Clamp(deltaAngle, -45, 45);
            
            ship.TurnWheel(deltaAngle);
            return false;
        }

        public bool Simulate(ManeuverContext context, float deltaTime)
        {
            var deltaAngle = _targetDirection - context.Self.PhysicsData.Rotation.eulerAngles.y;
            deltaAngle *= 2f;//rotation effectiveness?
            deltaAngle = Mathf.Clamp(deltaAngle, -45, 45);

            //TODO simulate update
            context.Self.Steering.Angle = deltaAngle;
            return false;
        }
    }
}