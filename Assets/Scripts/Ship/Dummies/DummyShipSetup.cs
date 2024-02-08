using System;
using Ship.Data;
using Ship.Interfaces;

namespace Ship.Dummies
{
    [Serializable]
    public class DummyShipSetup : IShipSetup
    {
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
    }
}