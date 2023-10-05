using System;
using Ship.AI.Data;
using Ship.Data;
using UnityEngine;

namespace Ship
{
    public static class ShipPhysics
    {
        public static (Vector3 linear, Vector3 angular) CalculateForces(ShipPhysicsData physics, ShipSteeringData steering, ShipRigData rigging, Vector3 wind)
        {
            var shipDirection = physics.Rotation * Vector3.forward;
            var velocity = physics.Velocity;
            var force = Vector3.zero;
            
            rigging.ForeachSail(sail =>
            {
                var vector = Quaternion.Euler(0, sail.Angle, 0) * shipDirection;
                force += vector * (sail.Input * sail.Setup);
            });
            
            var right = physics.Rotation * Vector3.right;
            
            var forceForward = shipDirection * (Vector3.Dot(force, shipDirection));
            //var forceRight = right * (Vector3.Dot(force, right) * 0.25f);
            //force = forceForward + forceRight;
            force = forceForward;//no side shifting at all - simplifies AI
            
            var steeringForce = Vector3.Dot(shipDirection, velocity) * steering.Angle * steering.Efficiency;
            var angularForce = new Vector3(0, steeringForce , 0);
            

            return (linear: force, angular: angularForce);
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

        public static void UpdateWindInput(ref ShipRigData data, Vector3 shipDirection, Vector3 wind)
        {
            SailSlot[] sailSlots = (SailSlot[])Enum.GetValues(typeof(SailSlot));

            // Iterate through the enum values
            foreach (SailSlot slot in sailSlots)
            {
                //TODO self shadowing
                var sail = data[slot];
                var vector = Quaternion.Euler(0, sail.Angle, 0) * shipDirection;
                var dotProduct = Vector3.Dot(vector, wind.normalized);
                sail.Input = wind.magnitude * dotProduct;
                data[slot] = sail;
            }
        }

        
        
    }
}