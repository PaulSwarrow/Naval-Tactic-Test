using System;
using System.Linq;
using DefaultNamespace.GameSystems;
using Ship.AI.Data;
using Ship.AI.Order;
using Ship.Data;
using UnityEngine;

namespace Ship.AI
{
    public abstract class BaseShipManeuver
    {
        public ManeuverPrediction Calculate(ManeuverContext context)
        {
            var result = new ManeuverPrediction();
            DoCalculation(context, result);
            return result;
        }

        protected abstract void DoCalculation(ManeuverContext context, ManeuverPrediction result);


        protected void CheckPoint(ManeuverContext context, ManeuverPrediction result, params IShipOrder[] orders)
        {
            context.ActiveOrders.AddRange(orders);
            result.Trajectory.Add(new ManeuverPredictionPhase()
            {
                Self = context.Self,
                Timestamp = context.Time,
                Orders = orders,
            });
        }

        protected void TurnTo(Vector3 direction, ManeuverContext context, ManeuverPrediction result)
        {
            var angle = Vector3.SignedAngle(context.Self.PhysicsData.Forward, direction, Vector3.up);
            var t = context.Time;
            while (Mathf.Abs(angle) > 1 && context.Time - t < 50)
            {
                var wind = context.Wind.GetWind(context.Self.PhysicsData.Position);
                var relativeWind = ShipPhysics.GetRelativeWind(wind, context.Self.PhysicsData.Forward);
                var order = angle > 0 ? SailOrder.TurnRight(relativeWind) : SailOrder.TurnLeft(relativeWind);
                CheckPoint(context, result, order.ToArray());
                FastForward(2, context, () =>
                {
                    var a = Vector3.SignedAngle(context.Self.PhysicsData.Forward, direction, Vector3.up);
                    var w = context.Wind.GetWind(context.Self.PhysicsData.Position);
                    var rw = ShipPhysics.GetRelativeWind(w, context.Self.PhysicsData.Forward);
                    return Mathf.Abs(a) < 1 || rw != relativeWind;
                });
                
                angle = Vector3.SignedAngle(context.Self.PhysicsData.Forward, direction, Vector3.up);
            }
            CheckPoint(context, result, SailOrder.Down(SailType.FrontJib), SailOrder.Down(SailType.Gaf));
        }
        
        protected void FastForward(float seconds, ManeuverContext context, Func<bool> stopCondition = null)
        {
            var deltaTime = Time.fixedDeltaTime; //performance concern

            var body = context.Self.PhysicsData;
            var endTime = context.Time + seconds;
            for (; context.Time < endTime; context.Time += deltaTime)
            {
                var wind = context.Wind.GetWind(body.Position); //assumption that wind doesn't change during time!
                var forces = ShipPhysics.CalculateForces(body, context.Self.Steering, context.Self.RigState, wind);
                
                var deceleration = ShipPhysics.CalculateHullDrag(body);
                // Calculate the acceleration due to drag force
                Vector3 acceleration = (forces.linear - body.Velocity * body.Drag) / body.Mass + deceleration;
                // Calculate the new velocity after timeToPredict
                Vector3 newVelocity = body.Velocity + acceleration * deltaTime;
                // Calculate the new position after timeToPredict
                var averageSpeed = body.Velocity + acceleration * deltaTime / 2;
                Vector3 newPosition = body.Position + averageSpeed * deltaTime;

                // Calculate the angular acceleration due to angular force
                Vector3 angularAcceleration =
                    (forces.angular - body.AngularVelocity * body.AngularDrag) / body.InertiaTensor;
                // Calculate the average angular velocity over the time interval
                Vector3 averageAngularVelocity = body.AngularVelocity + angularAcceleration * (deltaTime / 2);
                // Update the angular velocity using angular acceleration
                var newAngularVelocity = body.AngularVelocity + angularAcceleration * deltaTime;
                // Calculate the new rotation based on the average angular velocity
                Quaternion newRotation = body.Rotation * Quaternion.Euler(averageAngularVelocity * deltaTime);

                // Update Context
                body.Position = newPosition;
                body.Velocity = newVelocity;
                body.Rotation = newRotation;
                body.AngularVelocity = newAngularVelocity;
                context.Self.RelativeWind = ShipPhysics.GetRelativeWind(wind, body.Forward);
                context.Self.PhysicsData = body;

                for (int i = context.ActiveOrders.Count - 1; i >= 0; i--)
                {
                    var order = context.ActiveOrders[i];
                    var completed = order.Simulate(context, deltaTime);
                    if(completed) context.ActiveOrders.RemoveAt(i);
                }
                
                if (stopCondition != null && stopCondition()) return;
            }
        }
    }
}