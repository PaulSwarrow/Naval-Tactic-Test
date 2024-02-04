using Ship.AI.Data;
using Ship.Data;
using Ship.OrdersManagement;

namespace Ship.AI.Order
{
    internal class SailSetupCommand : IShipOrder
    {
        private readonly SailType _type;
        private readonly int _setup;


        public SailSetupCommand(SailType type, int setup)
        {
            _type = type;
            _setup = setup;
        }

        public bool Execute(ShipBody ship)
        {
            ship.SetupSail(_type, _setup);
            return true;//TODO continuous execution 
        }

        public bool Simulate(ManeuverContext context, float deltaTime)
        {
            ApplyTo(ref context.Self.Configuration);
            return true;//TODO continuous execution 
        }

        public void ApplyTo(ref ShipConfiguration configuration)
        {
            var state = configuration.Rigging[_type];
            state.Setup = _setup;
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