using System;

namespace Ship.Data
{
    [Serializable]
    public struct ShipRigData
    {
        public ShipRigSailsData MainSail;
        public ShipRigSailsData MiddleSail;
        public ShipRigSailsData MizzenSail;
        public ShipRigSailsData GafSail;
    }

    [Serializable]
    public struct ShipRigSailsData
    {
        public float Angle;
        public int Setup;
    }
}