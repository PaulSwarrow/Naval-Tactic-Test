using Ship.AI.Data;
using Ship.Data;
using Ship.OrdersManagement;

namespace Ship.AI.Order
{
    public class SteeringCommand : IShipOrder
    {
        private readonly int _angle;

        public SteeringCommand(int angle)
        {
            _angle = angle;
        }

        public bool Execute(ShipBody ship)
        {
            ship.TurnWheel(_angle);
            return false;
        }

        public bool Simulate(ManeuverContext context, float deltaTime)
        {
            ApplyTo(ref context.Ship.Configuration);
            return false;
        }

        public void ApplyTo(ref ShipConfiguration configuration)
        {
            var state = configuration.Steering;
            state.Angle = _angle;
            configuration.Steering = state;
        }

        public ShipCommandEstimation Estimate(ShipConfiguration configuration)
        {
            return new ShipCommandEstimation()
            {
                Seconds = 1,
                CrewUnits = 1
            };
        }
        
        public ShipOrderCategory Category => ShipOrderCategory.Steer;
    }
}