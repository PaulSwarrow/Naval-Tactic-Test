using DefaultNamespace.GameSystems;
using Ship.AI.Data;
using Ship.Data;
using Ship.View;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Ship
{
    public class ShipBody : MonoBehaviour
    {
        [Inject] private WindSystem _windSystem;

        [SerializeField] private float _keelDrag = 0.9f;

        [FormerlySerializedAs("_rigData")] [SerializeField] private ShipRigState rigState;
        [SerializeField] private ShipSteeringState _steering;


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
        private Vector3 _velocity;

        private void FixedUpdate()
        {
            var t = Time.fixedDeltaTime;
            var self = transform;
            var wind = _windSystem.GetWind(self.position); //assumption that wind doesn't change during time!
            var physicsData = GetPhysicsData();
            var configuration = new ShipConfiguration()
            {
                Rigging = rigState,
                Steering = _steering
            };
            var forces = ShipPhysics.CalculateForces(physicsData, configuration, wind);
            var deceleration = ShipPhysics.CalculateHullDrag(physicsData);
            
            //add linear force
            var mass = _body.mass;
            
            Vector3 acceleration = (forces.linear - _body.velocity * _body.drag) / _body.mass + deceleration;
            var averageSpeed = _velocity + acceleration * t / 2;
            Vector3 newPosition = _body.position + averageSpeed * t;
            
            _body.MovePosition(newPosition);
            _velocity = _velocity + acceleration * t;
            //_body.AddForce();
            
            //add angular force manually
            Vector3 angularAcceleration = (forces.angular - _angularVelocity * _body.angularDrag) / _body.inertiaTensor.magnitude;
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
                Configuration = new ShipConfiguration()
                {
                    Rigging = rigState,
                    Steering = _steering
                }
            };
        }
        public ShipPhysicsData GetPhysicsData()
        {
            var self = transform;
            return new ShipPhysicsData()
            {
                Position = self.position,
                Rotation = self.rotation,
                Velocity = _velocity,
                AngularVelocity = _angularVelocity,
                Drag = _body.drag,
                AngularDrag = _body.angularDrag,
                KeelDrag = _keelDrag,
                Mass = _body.mass,
                InertiaTensor = _body.inertiaTensor.magnitude
            };
        }
        
        public Quaternion Rotation => transform.rotation;
        
        private void OnGUI()
        {
            GUILayout.Label($" t: {_time}\np:{_body.position}\nv:{_velocity}\nr:{_body.rotation.eulerAngles.y}\nav{_angularVelocity.y}");
        }
        public void TurnWheel(float angle)
        {
            _steering.Angle = (int)angle;
        }

        public ShipSailState GetSailInfo(SailType sail)
        {
           return rigState[sail];
        }

        //TODO 
        public void SetupSail(SailType type, int value)
        {
            var sail = rigState[type];
            sail.Setup = value;
            rigState[type] = sail;
        }
        //TODO 
        public void TurnSail(SailType type, int angle)
        {
            var sail = rigState[type];
            sail.Angle = angle;
            rigState[type] = sail;
        }
    }
}