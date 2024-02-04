using Ship.AI.Data;
using Ship.Data;
using Ship.Interfaces;
using Ship.OrdersManagement;

namespace Ship.AI
{
    public interface IShipOrder
    {
        /// <summary>
        /// enables ship's systems to execute the command
        /// </summary>
        /// <param name="ship"></param>
        /// <returns>complete</returns>
        bool Execute(ShipBody ship);
        
        /// <summary>
        /// Simulates execution on abstract data
        /// </summary>
        /// <param name="context"></param>
        /// <param name="deltaTime"></param>
        /// <returns>complete</returns>
        bool Simulate(ManeuverContext context, float deltaTime);

        /// <summary>
        /// Changes ship configuration to a command's resulting state
        /// </summary>
        /// <param name="configuration"></param>
        void ApplyTo(ref ShipConfiguration configuration);

        /// <summary>
        /// Provides information for the Commands Composer
        /// TODO feed with more data about current ship & crew state
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        ShipCommandEstimation Estimate(ShipConfiguration configuration);
        ShipOrderCategory Category { get; }
    }
}