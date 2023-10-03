﻿using DefaultNamespace.GameSystems;
using Ship.AI.Data;
using Ship.Data;
using Ship.View;
using UnityEngine;
using Zenject;

namespace Ship
{
    public class ShipBody : MonoBehaviour
    {
        [Inject] private WindSystem _windSystem;

        [SerializeField] private float _keelDrag = 0.9f;

        [SerializeField] private ShipRigData _rigData;
        [SerializeField] private ShipSteeringData _steering;


        private SailView[] Sails;

        private Rigidbody _body;
        private float _time;

        private void Awake()
        {
            _body = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            Sails = GetComponentsInChildren<SailView>();
        }

        private void Update()
        {

            _time += Time.deltaTime;
            
            

        }

        private Vector3 _angularVelocity;

        private void FixedUpdate()
        {
            var t = Time.fixedDeltaTime;
            var self = transform;
            var wind = _windSystem.GetWind(self.position); //assumption that wind doesn't change during time!
            var physicsData = GetPhysicsData();
            var forces = ShipPhysics.CalculateForces(physicsData, _steering, _rigData, wind);
            //add linear force
            _body.AddForce(forces.linear);
            
            //add angular force manually
            Vector3 angularAcceleration = (forces.angularForce - _angularVelocity * _body.angularDrag) / _body.inertiaTensor.magnitude;
            Vector3 averageAngularVelocity = _angularVelocity + angularAcceleration * (t / 2);
            _angularVelocity = _angularVelocity + angularAcceleration * t;
            Quaternion rotationChange = Quaternion.Euler(averageAngularVelocity * t);
            _body.MoveRotation(_body.rotation * rotationChange);
        }

        public AIPredictionShipData ExportToAI()
        {
            return new AIPredictionShipData()
            {
                PhysicsData = GetPhysicsData(),
                RigData = _rigData,
                Steering = _steering,
            };
        }
        public ShipPhysicsData GetPhysicsData()
        {
            var self = transform;
            return new ShipPhysicsData()
            {
                Position = self.position,
                Rotation = self.rotation,
                Velocity = _body.velocity,
                AngularVelocity = _angularVelocity,
                Drag = _body.drag,
                AngularDrag = _body.angularDrag,
                Mass = _body.mass,
                InertiaTensor = _body.inertiaTensor.magnitude
            };
        }
        
        public Quaternion Rotation => transform.rotation;
        
        private void OnGUI()
        {
            GUILayout.Label($" t: {_time}\np:{_body.position}\nv:{_body.velocity}\nr:{_body.rotation.eulerAngles.y}\nav{_angularVelocity.y}");
        }
        public void TurnWheel(float angle)
        {
            _steering.Angle = angle;
        }
    }
}