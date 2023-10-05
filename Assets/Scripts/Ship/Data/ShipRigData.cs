using System;

namespace Ship.Data
{
    public enum SailSlot
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
    public struct ShipRigData
    {
        public delegate void SailHandler(ShipSailData sailData);

        public ShipSailData FrontJib;
        public ShipSailData MainSail;
        public ShipSailData MiddleSail;
        public ShipSailData MiddleJib;
        public ShipSailData MizzenSail;
        public ShipSailData GafSail;


        public void ForeachSail(SailHandler handler)
        {
            //TODO check existence
            handler.Invoke(FrontJib);
            handler.Invoke(MiddleJib);
            handler.Invoke(MainSail);
            handler.Invoke(MiddleSail);
            handler.Invoke(MizzenSail);
            handler.Invoke(GafSail);
        }

        public ShipSailData this[SailSlot key]
        {
            get => key switch
            {
                SailSlot.FrontJib => FrontJib,
                SailSlot.MiddleJib => MiddleJib,
                SailSlot.MainSail => MainSail,
                SailSlot.MiddleSail => MiddleSail,
                SailSlot.MizzenSail => MizzenSail,
                SailSlot.Gaf => GafSail,
                _ => default
            };
            set
            {
                switch (key)
                {
                    case SailSlot.FrontJib:
                        FrontJib = value;
                        break;
                    case SailSlot.MiddleJib:
                        MiddleJib = value;
                        break;
                    case SailSlot.MainSail:
                        MainSail = value;
                        break;
                    case SailSlot.MiddleSail:
                        MiddleSail = value;
                        break;
                    case SailSlot.MizzenSail:
                        MizzenSail = value;
                        break;
                    case SailSlot.Gaf:
                        GafSail = value;
                        break;
                }
            }
        }
    }

    [Serializable]
    public struct ShipSailData
    {
        public float Angle;
        public int Setup;
        public float Input;
    }
}