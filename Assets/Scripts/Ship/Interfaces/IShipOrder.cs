using Ship.AI.Data;
using Ship.OrdersManagement;

namespace Ship.AI
{
    public interface IShipOrder
    {
        //TODO executioners
        bool Execute(ShipBody ship);
        bool Simulate(ManeuverContext context, float deltaTime);
        ShipOrderCategory Category { get; }
    }
}