using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Ship.AI.SailSchematics;
using Ship.AI.Data;
using Ship.Data;
using Ship.Interfaces;
using Ship.OrdersManagement;
using Ship.Utils;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace Ship.AI.CommandsGraphSearch
{
    public class CommandsComposer
    {
        private readonly int[] _steeringAngles = new[] { 0, 45, 90, -45, -90 };

        private Dictionary<SailType, SailScheme[]> _rigScheme = new();

        public PathData<IShipOrder> Turn(ManeuverContext context, float deltaAngle)
        {
            PathData<IShipOrder> result = new();

            SailRiggingTemplate template;

            var estimatedTorque = 0;

            var wind = context.Wind.GetWind(context.Ship.PhysicsData.Position);
            var relativeWind = ShipPhysics.GetRelativeWind(context.Ship.PhysicsData.Rotation, wind);
            var relativeWindCourse = ShipPhysics.DirectionToCourse(relativeWind);

            var currentConfiguration = context.Ship.Configuration;
            foreach (var sail in context.ShipSetup.GetSails())
            {
                if (sail.UsedForTurns)
                {
                    var targetState = MaxTurn(sail, Mathf.Sign(deltaAngle), relativeWind);
                    result.AddRange(GetOrders(sail.SailType, currentConfiguration.Rigging[sail.SailType], targetState));
                    //Get optimum turn position with accumulated values
                }
                else
                {
                    //Select speed mode
                }
            }


            return result;
        }

        public PathData<IShipOrder> StopRotation(ManeuverContext context)
        {
            PathData<IShipOrder> result = new();

            var wind = context.Wind.GetWind(context.Ship.PhysicsData.Position);
            var relativeWind = ShipPhysics.GetRelativeWind(context.Ship.PhysicsData.Rotation, wind);
            var relativeWindCourse = ShipPhysics.DirectionToCourse(relativeWind);

            SailType[] orderedSails = EnumTools.ToArray<SailType>();
            foreach (var sailType in orderedSails)
            {
                var schemes = _rigScheme[sailType].First(s =>
                {
                    var info = s.GetInfo(relativeWindCourse);
                    //TODO more complex formula here!
                    return info.TorqueFactor == 0;
                });

                result.AddRange(schemes.OrdersToSet(context.Ship.Configuration.Rigging));
            }

            return result;
        }

        private ShipSailState MaxTurn(ISailSetup sail, float sign, Vector3 wind)
        {
            var resultState = new ShipSailState();
            double maxTorque = 0; //assumption that there is always zero torque configuration (setup = 0)

            foreach (var configuration in sail.GetAllConfigurations())
            {
                var forces = ShipPhysics.GetForces(configuration, sail, wind);
                if (forces.torque * sign > maxTorque * sign)
                {
                    resultState = configuration;
                    maxTorque = forces.torque;
                }
            }

            return resultState;
        }


        private IEnumerable<IShipOrder> GetOrders(SailType sailType, ShipSailState current, ShipSailState target)
        {
            var result = new List<IShipOrder>();
            //Do not turn lowered sail!
            if (current.Angle != target.Angle && target.Setup != 0)
                result.Add(ShipCommands.SailTurn(sailType, target.Angle));
            if (current.Setup != target.Setup) result.Add(ShipCommands.SailSetup(sailType, target.Setup));
            return result;
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