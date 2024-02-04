using System;
using System.Collections.Generic;
using Ship.Data;
using Ship.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ship.AI.Data
{
    public class ManeuverContext
    {
        public float Time;
        public List<IShipOrder> ActiveOrders = new List<IShipOrder>();
        public AIPredictionShipData Ship;

        public IWindProvider Wind;
        public IDepthProvider Depth;
        public IShipSetup ShipSetup;
    }

    [Serializable]
    public struct AIPredictionShipData
    {
        public ShipPhysicsData PhysicsData;
        [HideInInspector] public ShipConfiguration Configuration;
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