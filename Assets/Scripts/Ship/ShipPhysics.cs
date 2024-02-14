using System;
using Ship.AI.Data;
using Ship.Data;
using UnityEngine;

namespace Ship
{
    public static class ShipPhysics
    {
        private static float GlobalWindMultiplier = 0.01f;

        public static (Vector3 linear, Vector3 angular) CalculateForces(ShipPhysicsData physics,
            ShipConfiguration configuration, Vector3 wind)
        {
            var shipDirection = physics.Rotation * Vector3.forward;
            var velocity = physics.Velocity;
            var totalForce = Vector3.zero;
            var relativeWind = GetRelativeWind(physics.Rotation, wind);

            var steeringForce = 0f;

            configuration.Rigging.ForeachSail((type, sail) =>
            {
                var vector = Quaternion.Euler(0, sail.Angle, 0) * Vector3.forward;
                var dot = Vector3.Dot(vector, relativeWind);
                var force = vector.normalized * (dot * sail.Setup);
                totalForce += force;
                var forceRight = Vector3.Dot(force, Vector3.right);
                if (type == SailType.FrontJib)
                {
                    steeringForce += forceRight * 50;
                }

                if (type == SailType.Gaf) steeringForce -= forceRight * 50;
            });

            var forceForward = Vector3.forward * Vector3.Dot(totalForce, shipDirection);
            //var forceRight = right * (Vector3.Dot(force, right) * 0.25f);
            //force = forceForward + forceRight;
            totalForce = forceForward; //no side shifting at all - simplifies AI

            steeringForce += Vector3.Dot(shipDirection, velocity) * configuration.Steering.Angle * 50;
            var angularForce = new Vector3(0, steeringForce, 0);

            return (linear: totalForce * GlobalWindMultiplier, angular: angularForce * GlobalWindMultiplier);
        }

        public static Vector3 GetRelativeWind(Quaternion shipRotation, Vector3 wind)
        {
            return Quaternion.Euler(0, -shipRotation.eulerAngles.y, 0) * wind;
        }

        public static Vector3 CalculateHullDrag(ShipPhysicsData physics)
        {
            var acceleration = Vector3.zero;
            var right = physics.Rotation * Vector3.right;
            var keelDotProduct = Vector3.Dot(right, physics.Velocity);
            var keelResistance = right * (keelDotProduct * physics.KeelDrag); //TODO magic number for keel drag force!
            acceleration -= keelResistance;

            var back = -(physics.Rotation * Vector3.forward);
            var sternDotProduct = Mathf.Max(Vector3.Dot(back, physics.Velocity), 0);
            var sternResistance = back * (sternDotProduct * 0.99f);
            acceleration -= sternResistance;

            return acceleration;
        }

        public static WorldDirection GetRelativeWind(Vector3 wind, Vector3 shipForward)
        {
            var angle = Vector3.SignedAngle(shipForward, wind, Vector3.up);
            var angleInt = Mathf.RoundToInt(angle / 45);
            switch (angleInt)
            {
                case 0: return WorldDirection.N;
                case 1: return WorldDirection.NE;
                case 2: return WorldDirection.E;
                case 3: return WorldDirection.SE;
                case 4:
                case -4: return WorldDirection.S;
                case -1: return WorldDirection.NW;
                case -2: return WorldDirection.W;
                case -3: return WorldDirection.SW;
                default: throw new Exception($"Wrong wind angle: {angle}");
            }
        }


        public static WorldDirection DirectionToCourse(Vector3 direction)
        {
            var angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
            angle /= 45;
            var intAngle = Mathf.RoundToInt(angle) * 45;
            if (intAngle == -180) intAngle = 180;
            return (WorldDirection)intAngle;
        }
    }
}