using System.Security.Cryptography;
using Ship.AI.Data;
using Ship.Data;
using Ship.OrdersManagement;

namespace Ship.AI.Order
{
    public class SailTurnCommand : IShipOrder
    {
        private readonly SailType _type;
        private readonly int _angle;

        public SailTurnCommand(SailType type, int angle)
        {
            _type = type;
            _angle = angle;
        }

        public bool Execute(ShipBody ship)
        {
            ship.TurnSail(_type, _angle);
            return true;
        }

        public bool Simulate(ManeuverContext context, float deltaTime)
        {
            ApplyTo(ref context.Ship.Configuration);
            return true;
        }
        public void ApplyTo(ref ShipConfiguration configuration)
        {
            var state = configuration.Rigging[_type];
            state.Angle = _angle;
            configuration.Rigging[_type] = state;
        }

        public ShipCommandEstimation Estimate(ShipConfiguration configuration)
        {
            return new ShipCommandEstimation()
            {
                Seconds = 1,
                CrewUnits = 1
            };
        }

        public ShipOrderCategory Category => ShipOrderCategory.Sails;
    }
}