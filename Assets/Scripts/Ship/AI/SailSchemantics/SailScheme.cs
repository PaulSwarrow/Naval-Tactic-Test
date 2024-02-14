using System.Collections.Generic;
using Ship.Data;
using Ship.Interfaces;
using Ship.OrdersManagement;
using UnityEngine;

namespace Ship.AI.SailSchemantics
{
    /// <summary>
    /// Represents ...
    /// </summary>
    public class SailScheme
    {
        private SailType _sail;
        
        public class SchemeInfo
        {
            public int Setup;
            public int Angle;
            public int TorqueFactor;
            public int LinearFactor;
        }
        
        private Dictionary<WorldDirection, SchemeInfo> _windMap;


        public SchemeInfo GetInfo(WorldDirection relativeWind)
        {
            return _windMap[relativeWind];
        }


        public IEnumerable<IShipOrder> OrdersToSet(ShipRigState configuration)
        {
            var result = new List<IShipOrder>();
            if (configuration[_sail].Angle != _angle) result.Add(ShipCommands.SailTurn(_sail, _angle));
            if (configuration[_sail].Setup != _setup) result.Add(ShipCommands.SailSetup(_sail, _setup));
            return result;
        }

        public SchemeInfo GetMostTorque()
        {
            
        }
        
        
    }
}