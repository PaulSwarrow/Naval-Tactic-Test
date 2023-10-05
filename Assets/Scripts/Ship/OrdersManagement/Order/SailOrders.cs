using System;
using System.Collections.Generic;
using Ship.AI.Data;
using Ship.Data;
using Ship.OrdersManagement;
using Unity.VisualScripting;

namespace Ship.AI.Order
{
    public class SailOrder : IShipOrder
    {
        public IShipOrder JibRight() => new SailOrder(SailSlot.FrontJib, 1, 45);
        public IShipOrder JibLeft() => new SailOrder(SailSlot.FrontJib, 1, 135);
        public IShipOrder SpankerRight() => new SailOrder(SailSlot.FrontJib, 1, 45);
        public IShipOrder SpankerLeft() => new SailOrder(SailSlot.FrontJib, 1, 135);


        public IShipOrder Up(SailSlot slot) => new SailOrder(slot, 1);
        public IShipOrder Down(SailSlot slot) => new SailOrder(slot, 0);


        public IEnumerable<IShipOrder> TurnRight(WorldDirection windRelative)
        {
            var result = new List<IShipOrder>();
            
            //spanker
            switch (windRelative)
            {
                case WorldDirection.N:
                case WorldDirection.NE:
                case WorldDirection.E:
                    result.Add(Up(SailSlot.Gaf));
                    result.Add(SpankerLeft());
                    break;
                case WorldDirection.SE:
                case WorldDirection.S:
                    result.Add(Up(SailSlot.Gaf));
                    result.Add(SpankerRight());
                    break;
                default:
                    result.Add(Down(SailSlot.Gaf));
                    break;
            }
            //jibs
            switch (windRelative)
            {
                case WorldDirection.N:
                case WorldDirection.NW:
                case WorldDirection.W:
                    result.Add(Up(SailSlot.FrontJib));
                    result.Add(SpankerRight());
                    break;
                case WorldDirection.SW:
                case WorldDirection.S:
                    result.Add(Up(SailSlot.FrontJib));
                    result.Add(SpankerLeft());
                    break;
                default:
                    result.Add(Down(SailSlot.FrontJib));
                    break;
            }

            return result;
        }public IEnumerable<IShipOrder> TurnLeft(WorldDirection windRelative)
        {
            var result = new List<IShipOrder>();
            
            //spanker
            switch (windRelative)
            {
                case WorldDirection.N:
                case WorldDirection.NW:
                case WorldDirection.W:
                    result.Add(Up(SailSlot.Gaf));
                    result.Add(SpankerRight());
                    break;
                case WorldDirection.SW:
                case WorldDirection.S:
                    result.Add(Up(SailSlot.Gaf));
                    result.Add(SpankerLeft());
                    break;
                default:
                    result.Add(Down(SailSlot.Gaf));
                    break;
            }
            //jibs
            switch (windRelative)
            {
                case WorldDirection.N:
                case WorldDirection.NE:
                case WorldDirection.E:
                    result.Add(Up(SailSlot.FrontJib));
                    result.Add(SpankerLeft());
                    break;
                case WorldDirection.SE:
                case WorldDirection.S:
                    result.Add(Up(SailSlot.FrontJib));
                    result.Add(SpankerRight());
                    break;
                default:
                    result.Add(Down(SailSlot.FrontJib));
                    break;
            }

            return result;
        }

        private SailSlot _sailSlot;
        private int _setup;
        public int _angle;

        public SailOrder(SailSlot sailSlot, int setup, int angle = 0)
        {
            _sailSlot = sailSlot;
            _setup = setup;
            _angle = angle;
        }

        public bool Execute(ShipBody ship)
        {
            ship.SetupSail(_sailSlot,_setup,_angle);
            return true;
        }

        public bool Simulate(ManeuverContext context, float deltaTime)
        {
            var sail = context.Self.RigData[_sailSlot];
            sail.Setup = _setup;
            sail.Angle = _angle;
            return true;
        }

        public ShipOrderCategory Category => ShipOrderCategory.Sails;
    }
    
    
}