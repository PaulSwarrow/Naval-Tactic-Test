using Ship.Interfaces;
using UnityEngine;

namespace DefaultNamespace.GameSystems
{
    public class WindSystem : MonoBehaviour, IWindProvider
    {
        [SerializeField]
        private float _direction;

        [SerializeField] private float _force = 1;
        public Vector3 GetWind(Vector3 position) => Quaternion.Euler(0, _direction, 0) * Vector3.forward * _force;
    }
}