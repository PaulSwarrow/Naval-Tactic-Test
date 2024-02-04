using System.Collections.Generic;
using Ship.AI.Data;
using Ship.AI.Order;
using Ship.Interfaces;
using Ship.OrdersManagement;

namespace Ship.AI.CommandsGraphSearch
{
    public class CommandsComposer : PathfindingAlgorithm<ManeuverNode, ManeuverEdge>
    {
        private readonly int[] SteeringAngles = new[] { 0, 45, 90, -45, -90 };

        public ShipPhysicsData ShipData;
        public IShipSetup Setup;
        public IWindProvider WindProvider;
        
        

        protected override int Heuristic(ManeuverNode node, int totalCost)
        {
        }

        protected override List<EdgeInfo> GetEdges(ManeuverNode node)
        {
            var edges = new List<EdgeInfo>();
            //generate sail commands
            node.Configuration.Rigging.ForeachSail((type, state) =>
            {
                EdgeInfo entry;
                //change angle commands
                foreach (var angle in Setup.SailAnglesAvailable(type))
                {
                    if (angle != state.Angle && state.Setup == 0)
                    {
                        
                        entry = new EdgeInfo();
                        entry.Edge = new ManeuverEdge();
                        entry.Edge.Order = ShipCommands.SailTurn(type, angle);
                        entry.Destination = GetEdgeDestination(node, entry.Edge);
                        edges.Add(entry);
                    }
                }

                //change setup commands
                foreach (var setup in Setup.SailSetupsAvailable(type))
                {
                    if (setup != state.Setup)
                    {
                        var nextState = state;
                        nextState.Setup = setup;
                        
                        entry = new EdgeInfo();
                        entry.Edge = new ManeuverEdge();
                        entry.Edge.Order = ShipCommands.SailSetup(type, setup);
                        entry.Destination = GetEdgeDestination(node, entry.Edge);
                        entry.Cost = GetGeneralCost(node, entry.Edge);
                        edges.Add(entry);
                    }
                }
            });

            
            foreach (var angle in SteeringAngles)
            {
                var entry = new EdgeInfo()
                {
                    Edge = new ManeuverEdge()
                    {
                        Order = ShipCommands.Steer(angle)
                    }
                };
                entry.Destination = GetEdgeDestination(node, entry.Edge);
                entry.Cost = GetGeneralCost(node, entry.Edge);
                edges.Add(entry);

            }

            return edges;
        }

        private int GetGeneralCost(ManeuverNode node, ManeuverEdge edge)
        {
            var estimation = edge.Order.Estimate(node.Configuration);
            return estimation.Seconds + (estimation.CrewUnits * estimation.Risk);
        }
        

        private ManeuverNode GetEdgeDestination(ManeuverNode node, ManeuverEdge edge)
        {
            var wind = WindProvider.GetWind(ShipData.Position);
            var destination = node;
            edge.Order.ApplyTo(ref destination.Configuration);
            var forces = ShipPhysics.CalculateForces(ShipData, destination.Configuration, wind);
            destination.LinearForce = (int)forces.linear.z;
            destination.AngularForce = (int)forces.angular.y;
            return destination;
        }

        protected override bool IsDestination(ManeuverNode node)
        {
        }
    }
}