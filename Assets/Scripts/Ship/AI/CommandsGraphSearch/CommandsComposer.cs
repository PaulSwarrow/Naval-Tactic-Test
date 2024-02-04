using System.Collections.Generic;
using Ship.AI.Data;
using Ship.Data;
using Ship.Interfaces;
using Ship.OrdersManagement;
using UnityEngine;

namespace Ship.AI.CommandsGraphSearch
{
    public class CommandsComposer : PathfindingAlgorithm<ManeuverNode, ManeuverEdge>
    {
        private readonly int[] SteeringAngles = new[] { 0, 45, 90, -45, -90 };

        public ShipPhysicsData ShipData;
        public IShipSetup Setup;
        public IWindProvider WindProvider;


        public PathData<ManeuverNode, ManeuverEdge> Turn(ManeuverContext context, RotationDirection direction)
        {
            var start = CreateInitialNode(context);
            return FindBest(start,
                (node) => direction == RotationDirection.Right ? node.AngularForce : -node.AngularForce);
        }

        public PathData<ManeuverNode, ManeuverEdge> StopRotation(ManeuverContext context)
        {
            var start = CreateInitialNode(context);
            return FindBest(start,
                node => int.MaxValue - Mathf.Abs(node.AngularForce));
        }

        public PathData<ManeuverNode, ManeuverEdge> FullForward(ManeuverContext context)
        {
            var start = CreateInitialNode(context);
            return FindBest(start, node => node.LinearForce - Mathf.Abs(node.AngularForce));
        }

        public PathData<ManeuverNode, ManeuverEdge> FullStop(ManeuverContext context)
        {
            var start = CreateInitialNode(context);
            return FindBest(start, node => int.MaxValue - Mathf.Abs(node.LinearForce));
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

        private ManeuverNode CreateInitialNode(ManeuverContext context)
        {
            var wind = WindProvider.GetWind(ShipData.Position);
            var forces = ShipPhysics.CalculateForces(ShipData, context.Self.Configuration, wind);
            return new ManeuverNode()
            {
                LinearForce = (int)forces.linear.z,
                AngularForce = (int)forces.angular.y,
                Configuration = context.Self.Configuration
            };
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
    }
}