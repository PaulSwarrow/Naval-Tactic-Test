using System;
using Ship.Data;
using UnityEngine;

namespace Ship.View
{
    [ExecuteInEditMode]
    public class SailView : MonoBehaviour
    {
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
            if (Application.isPlaying)
            {
                var info = _ship.GetSailInfo(_sailData);
                Setup = info.Setup;
                InputWind = info.Input;
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