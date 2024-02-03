using System;
using Ship.AI.Data;
using Ship.Data;
using UnityEngine;

namespace Ship.AI.CommandsGraphSearch
{
    public struct ManeuverNode : IEquatable<ManeuverNode>
    {
        public ShipPhysicsData Body;
        public ShipRigState RigState;
        public ShipSteeringState SteeringState;

        private ManeuverNodeShort GetShort()
        {
            return new ManeuverNodeShort()
            {
                Position = new Vector2Int((int)Body.Position.x, (int)Body.Position.z),
                Rotation = (int)Body.Rotation.y,
                Velocity = new Vector2Int((int)Body.Velocity.x, (int)Body.Velocity.z),
                AngularVelocity = (int)Body.AngularVelocity.y,
                RigState = RigState,
                SteeringState = SteeringState
            };
        }

        public bool Equals(ManeuverNode other)
        {
            return GetShort().Equals(other.GetShort());
        }

        public override bool Equals(object obj)
        {
            return obj is ManeuverNode other && Equals(other);
        }

        public override int GetHashCode()
        {
            return GetShort().GetHashCode();
        }
    }

    public struct ManeuverNodeShort
    {
        public Vector2Int Position;
        public int Rotation;
        public Vector2Int Velocity;
        public int AngularVelocity;
        public ShipRigState RigState;
        public ShipSteeringState SteeringState;
    }
}