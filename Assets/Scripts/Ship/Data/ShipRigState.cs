using System;
using UnityEngine;

namespace Ship.Data
{
    public enum SailType
    {
        None,
        FrontJib,
        MiddleJib,
        MainSail,
        MiddleSail,
        MizzenSail,
        Gaf //spanker
    }

    [Serializable]
    public struct ShipRigState
    {
        public delegate void SailHandler(SailType type, ShipSailState sailState);

        public ShipSailState FrontJib;
        public ShipSailState MainSail;
        public ShipSailState MiddleSail;
        public ShipSailState MiddleJib;
        public ShipSailState MizzenSail;
        public ShipSailState GafSail;


        public void ForeachSail(SailHandler handler)
        {
            //TODO check existence
            handler.Invoke(SailType.FrontJib, FrontJib);
            handler.Invoke(SailType.MiddleJib, MiddleJib);
            handler.Invoke(SailType.MainSail, MainSail);
            handler.Invoke(SailType.MiddleSail, MiddleSail);
            handler.Invoke(SailType.MizzenSail, MizzenSail);
            handler.Invoke(SailType.Gaf, GafSail);
        }

        public ShipSailState this[SailType key]
        {
            get => key switch
            {
                SailType.FrontJib => FrontJib,
                SailType.MiddleJib => MiddleJib,
                SailType.MainSail => MainSail,
                SailType.MiddleSail => MiddleSail,
                SailType.MizzenSail => MizzenSail,
                SailType.Gaf => GafSail,
                _ => default
            };
            set
            {
                switch (key)
                {
                    case SailType.FrontJib:
                        FrontJib = value;
                        break;
                    case SailType.MiddleJib:
                        MiddleJib = value;
                        break;
                    case SailType.MainSail:
                        MainSail = value;
                        break;
                    case SailType.MiddleSail:
                        MiddleSail = value;
                        break;
                    case SailType.MizzenSail:
                        MizzenSail = value;
                        break;
                    case SailType.Gaf:
                        GafSail = value;
                        break;
                }
            }
        }
    }

    [Serializable]
    public struct ShipSailState
    {
        public int Angle;
        public int Setup;
        public float GetInput(Vector3 relativeWind)
        {
            var v = Quaternion.Euler(0, Angle, 0) * Vector3.forward;
            var dotProduct = Vector3.Dot(v, relativeWind.normalized);
            return relativeWind.magnitude * dotProduct;
        }
    }

    [Serializable]
    public struct ShipSteeringState
    {
        public int Angle;
    }
}