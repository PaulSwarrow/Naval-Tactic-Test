using Ship.AI.Data;
using Ship.Data;
using UnityEngine;

namespace Ship
{
    public static class ShipPhysics
    {
        public static (Vector3 linear, Vector3 angularForce) CalculateForces(ShipPhysicsData physics, ShipSteeringData steering, ShipRigData rigging, Vector3 wind)
        {
            var shipDirection = physics.Rotation * Vector3.forward;
            var velocity = physics.Velocity;
            var angularVelocity = physics.AngularVelocity;
            var force = Vector3.zero;
            

            rigging.ForeachSail(sail =>
            {
                var vector = Quaternion.Euler(0, sail.Angle, 0) * shipDirection;
                var dotProduct = Vector3.Dot(vector, wind.normalized);
                var sailForce = vector * (wind.magnitude * dotProduct);
                force += sailForce;
            });
            
            var right = physics.Rotation * Vector3.right;
            var keelDotProduct = Vector3.Dot(right, velocity);
            var keelResistance = right * (keelDotProduct * 0.9f); //TODO magic number for keel drag force!
            force -= keelResistance;

            var back = -shipDirection;
            var sternDotProduct = Mathf.Max(Vector3.Dot(back, velocity), 0);
            var sternResistance = back * (sternDotProduct * 0.99f);
            force -= sternResistance;

            var steeringForce = Vector3.Dot(shipDirection, velocity) * steering.Angle * steering.Efficiency;
            var angularForce = new Vector3(0, steeringForce , 0);
            

            return (linear: force, angularForce: angularForce);
        }

        public delegate void SailHandler(ShipRigSailsData sailData);

        public static void ForeachSail(this ShipRigData data, SailHandler handler)
        {
            //TODO check existence
            handler.Invoke(data.MainSail);
            handler.Invoke(data.MiddleSail);
            handler.Invoke(data.MizzenSail);
            handler.Invoke(data.GafSail);
        }
    }
}