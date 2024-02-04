using Ship.AI;
using Ship.AI.Data;
using Ship.AI.Order;
using Ship.Data;
using UnityEngine;

namespace Ship.OrdersManagement
{
    public static class ShipCommands
    {
        public static IShipOrder SailSetup(SailType type, int setup) => new SailSetupCommand(type, setup);

        public static IShipOrder SailTurn(SailType type, int angle) => new SailTurnCommand(type, angle);

        public static IShipOrder Steer(int angle) => new SteeringCommand(angle);

        public static IShipOrder KeepCourse(Vector3 direction) => new KeepCourseCommand(direction);

    }

    
    
    
}