﻿using System.Collections.Generic;
using Ship.AI.Data;
using Ship.Data;
using Ship.OrdersManagement;
using UnityEngine;

namespace Ship.AI.CommandsGraphSearch
{
    public class CommandsComposer : PathfindingAlgorithm<ManeuverNode, ManeuverEdge, ManeuverContext, IShipOrder>
    {
        private readonly int[] _steeringAngles = new[] { 0, 45, 90, -45, -90 };

        public PathData<IShipOrder> Turn(ManeuverContext context, RotationDirection direction)
        {
            var start = CreateInitialNode(context);
            return FindBest(start, context, 
                (node) => direction == RotationDirection.Right ? node.AngularForce : -node.AngularForce);
        }

        public PathData<IShipOrder> StopRotation(ManeuverContext context)
        {
            var start = CreateInitialNode(context);
            return FindBest(start, context,
                node => int.MaxValue - Mathf.Abs(node.AngularForce));
        }

        public PathData<IShipOrder> FullForward(ManeuverContext context)
        {
            var start = CreateInitialNode(context);
            return FindBest(start, context, node => node.LinearForce - Mathf.Abs(node.AngularForce));
        }

        public PathData<IShipOrder> FullStop(ManeuverContext context)
        {
            var start = CreateInitialNode(context);
            return FindBest(start, context,node => int.MaxValue - Mathf.Abs(node.LinearForce));
        }


        protected override List<EdgeInfo> GetEdges(ManeuverNode node, ManeuverContext context)
        {
            var edges = new List<EdgeInfo>();
            //generate sail commands
            node.Configuration.Rigging.ForeachSail((type, state) =>
            {
                EdgeInfo entry;
                //change angle commands
                foreach (var angle in context.ShipSetup.SailAnglesAvailable(type))
                {
                    if (angle != state.Angle && state.Setup == 0)
                    {
                        entry = new EdgeInfo();
                        entry.Edge = new ManeuverEdge();
                        entry.Edge.Order = ShipCommands.SailTurn(type, angle);
                        entry.Destination = GetEdgeDestination(node, entry.Edge, context);
                        edges.Add(entry);
                    }
                }

                //change setup commands
                foreach (var setup in context.ShipSetup.SailSetupsAvailable(type))
                {
                    if (setup != state.Setup)
                    {
                        var nextState = state;
                        nextState.Setup = setup;

                        entry = new EdgeInfo();
                        entry.Edge = new ManeuverEdge();
                        entry.Edge.Order = ShipCommands.SailSetup(type, setup);
                        entry.Destination = GetEdgeDestination(node, entry.Edge, context);
                        entry.Cost = GetGeneralCost(node, entry.Edge);
                        edges.Add(entry);
                    }
                }
            });


            foreach (var angle in _steeringAngles)
            {
                var entry = new EdgeInfo()
                {
                    Edge = new ManeuverEdge()
                    {
                        Order = ShipCommands.Steer(angle)
                    }
                };
                entry.Destination = GetEdgeDestination(node, entry.Edge, context);
                entry.Cost = GetGeneralCost(node, entry.Edge);
                edges.Add(entry);
            }

            return edges;
        }

        protected override IShipOrder CreatePathElement(ManeuverNode origin, ManeuverEdge step)
        {
            return step.Order;
        }

        private ManeuverNode CreateInitialNode(ManeuverContext context)
        {
            var wind = context.Wind.GetWind(context.Ship.PhysicsData.Position);
            var forces = ShipPhysics.CalculateForces(context.Ship.PhysicsData, context.Ship.Configuration, wind);
            return new ManeuverNode()
            {
                LinearForce = (int)forces.linear.z,
                AngularForce = (int)forces.angular.y,
                Configuration = context.Ship.Configuration
            };
        }

        private int GetGeneralCost(ManeuverNode node, ManeuverEdge edge)
        {
            var estimation = edge.Order.Estimate(node.Configuration);
            return estimation.Seconds + (estimation.CrewUnits * estimation.Risk);
        }
        
        private ManeuverNode GetEdgeDestination(ManeuverNode node, ManeuverEdge edge, ManeuverContext context)
        {
            var wind = context.Wind.GetWind(context.Ship.PhysicsData.Position);
            var destination = node;
            edge.Order.ApplyTo(ref destination.Configuration);
            var forces = ShipPhysics.CalculateForces(context.Ship.PhysicsData, destination.Configuration, wind);
            destination.LinearForce = (int)(forces.linear.z * 1000);
            destination.AngularForce = (int)(forces.angular.y * 1000);
            return destination;
        }
    }
}