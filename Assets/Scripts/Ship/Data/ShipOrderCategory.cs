using System;

namespace Ship.OrdersManagement
{
    [Flags]
    public enum ShipOrderCategory
    {
        None, //for debug
        Sails, //square sails
        Steer, //also includes spanker and jibs rigging
        Guns,
        //etc
    }
}