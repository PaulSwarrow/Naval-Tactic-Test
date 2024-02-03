using System;

namespace Ship.Data
{
    [Serializable]
    public class ShipConfiguration
    {
        [Serializable]
        public class SailConfig
        {
            public SailType Type;
            public int[] Angles;
            public int Count;
        }
        
        public ShipConfiguration[] Sails;

    }
}