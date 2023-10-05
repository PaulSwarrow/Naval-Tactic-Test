using System;
using System.Collections.Generic;
using Ship.Data;
using Ship.Interfaces;
using UnityEngine;

namespace Ship.AI.Data
{
    public class ManeuverContext
    {
        public float Time;
        public List<IShipOrder> ActiveOrders = new List<IShipOrder>();
        public AIPredictionShipData Self;

        public IWindProvider Wind;
        public IDepthProvider Depth;
    }

    [Serializable]
    public struct AIPredictionShipData
    {
        public ShipPhysicsData PhysicsData;
        [HideInInspector]
        public ShipRigData RigData;
        public ShipSteeringData Steering;
        public WorldDirection RelativeWind;
    }

    [Serializable]
    public struct ShipSteeringData
    {
        public float Angle;
        public float Efficiency;
    }
    
    [Serializable]
    public struct ShipPhysicsData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Velocity;
        public Vector3 AngularVelocity;
        public float Mass;
        public float InertiaTensor;
        public float Drag;
        public float AngularDrag;
        public float KeelDrag;
        public Vector3 Forward => Rotation * Vector3.forward;
    }
}