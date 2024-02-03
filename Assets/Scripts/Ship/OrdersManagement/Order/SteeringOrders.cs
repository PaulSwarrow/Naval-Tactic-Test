using Ship.AI.Data;
using Ship.OrdersManagement;
using UnityEngine;

namespace Ship.AI.Order
{
    public class SteeringOrders : IShipOrder
    {

        public static IShipOrder KeepCourse(Vector3 direction) => new SteeringOrders(direction);
        
        

        private Vector3 _targetDirection;

        private SteeringOrders(Vector3 targetDirection)
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
            var forward = context.Self.PhysicsData.Rotation * Vector3.forward;
            var direction = _targetDirection;
            var deltaAngle = Vector3.SignedAngle(forward, direction, Vector3.up);
            deltaAngle *= 2f;//rotation effectiveness?
            deltaAngle = Mathf.Clamp(deltaAngle, -45, 45);

            //TODO simulate update
            context.Self.Steering.Angle = deltaAngle;
            return false;
        }
    }
}