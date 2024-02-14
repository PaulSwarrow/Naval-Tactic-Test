using System;
using System.Collections.Generic;
using Ship.AI.SailSchemantics;
using Ship.Data;
using Ship.Interfaces;
using Ship.Utils;
using UnityEngine;

namespace Ship.Dummies
{
    [Serializable]
    public class DummyShipSetup : IShipSetup
    {
        private Dictionary<SailType, SailScheme[]> _schemes;

        public DummyShipSetup()
        {
            _schemes = new();
            foreach (var sailType in GetAllSails())
            {
                foreach (var angle in SailAnglesAvailable(sailType))
                {
                    foreach (var setup in SailSetupsAvailable(sailType))
                    {
                        foreach (var direction in EnumTools.ToArray<WorldDirection>())
                        {
                            var wind = Quaternion.Euler(0, (int)direction, 0) * Vector3.forward;
                            var forwardFactor = Vector3.Dot(wind, Vector3.forward);
                            var strafeFactor = Vector3.Dot(wind, Vector3.right);
                            
                            var sailLinearMultiplier = 1;//TODO
                            var force = forwardFactor * sailLinearMultiplier;
                            var linearFactor = Mathf.Abs(force * 100);
                            
                            
                            
                        }
                        
                    }
                }
            }
        }


        public int[] SailAnglesAvailable(SailType type)
        {
            switch (type)
            {
                case SailType.FrontJib:
                case SailType.MiddleJib:
                case SailType.Gaf:
                    return new[] { 45, 60, 90, 120, 135 };
                default:
                    return new[] { 0, 30, -30, 50, -50 };
            }
        }

        public int[] SailSetupsAvailable(SailType type)
        {
            switch (type)
            {
                case SailType.FrontJib:
                case SailType.MiddleJib:
                case SailType.Gaf:
                    return new[] { 0, 1 };
                default:
                    return new[] { 0, 1, 2 };
            }
        }

        public SailType[] GetAllSails()
        {
            return new[]
            {
                SailType.FrontJib,
                SailType.MiddleJib,
                SailType.MainSail,
                SailType.MiddleSail,
                SailType.MizzenSail,
                SailType.Gaf
            };
        }

        public IReadOnlyDictionary<SailType, SailScheme[]> GetSchemes()
        {
            
        }
    }
}