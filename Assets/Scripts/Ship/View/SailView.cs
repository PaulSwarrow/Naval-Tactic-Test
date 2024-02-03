using System;
using DefaultNamespace.GameSystems;
using Ship.Data;
using UnityEngine;
using Zenject;

namespace Ship.View
{
    [ExecuteInEditMode]
    public class SailView : MonoBehaviour
    {
        [Inject] private WindSystem _windSystem;
        [Range(0.1f,1)]
        public float InputWind;

        [Range(0,2)]
        public int Setup;
        
        public float Angle;
        
        [SerializeField] private Transform[] _sails;

        [SerializeField] private SailSlot _sailData;
        
        private ShipBody _ship;

        private void Awake()
        {
            _ship = GetComponentInParent<ShipBody>();
        }

        private void Update()
        {
            var wind = _windSystem.GetWind(transform.position);
            var relativeWind = Quaternion.Euler(0, -transform.rotation.eulerAngles.y, 0) *wind;
            
            if (Application.isPlaying)
            {
                var info = _ship.GetSailInfo(_sailData);
                Setup = info.Setup;
                InputWind = info.GetInput(relativeWind);
                Angle = info.Angle;
            }
            if(_sails == null) return;

            for (int i = 0; i < _sails.Length; i++)
            {
                var sail = _sails[i];
                sail.gameObject.SetActive(Setup > i);
                sail.localScale = new Vector3(1, InputWind, 1);
            }
            
            transform.localRotation = Quaternion.Euler(0, Angle, 0);
        }
        
    }
}