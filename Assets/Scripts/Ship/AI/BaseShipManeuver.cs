using System;
using DefaultNamespace.GameSystems;
using Ship.AI.Data;
using UnityEngine;

namespace Ship.AI
{
    public abstract class BaseShipManeuver
    {
        public WindSystem _windSystem;


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
        protected void FastForward(float seconds, ManeuverContext context, Func<bool> stopCondition = null)
        {
            var deltaTime = Time.fixedDeltaTime; //performance concern

            var body = context.Self.PhysicsData;
            var endTime = context.Time + seconds;
            for (; context.Time < endTime; context.Time += deltaTime)
            {
                var wind = _windSystem.GetWind(body.Position); //assumption that wind doesn't change during time!
                ShipPhysics.UpdateWindInput(ref context.Self.RigData, body.Rotation * Vector3.forward ,wind);
                var forces = ShipPhysics.CalculateForces(body, context.Self.Steering, context.Self.RigData, wind);
                
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