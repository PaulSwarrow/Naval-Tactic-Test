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
        public static IShipOrder JibRight() => new SailOrder(SailType.FrontJib, 1, 45);
        public static IShipOrder JibLeft() => new SailOrder(SailType.FrontJib, 1, 135);
        public static IShipOrder SpankerRight() => new SailOrder(SailType.Gaf, 1, 45);
        public static IShipOrder SpankerLeft() => new SailOrder(SailType.Gaf, 1, 135);
        public static IShipOrder Up(SailType type) => new SailOrder(type, 1);
        public static IShipOrder Down(SailType type) => new SailOrder(type, 0);
        
        public static IEnumerable<IShipOrder> TurnRight(WorldDirection windRelative)
        {
            var result = new List<IShipOrder>();
            
            //spanker
            switch (windRelative)
            {
                case WorldDirection.N:
                case WorldDirection.NW:
                case WorldDirection.W:
                    result.Add(SpankerLeft());
                    break;
                case WorldDirection.SW:
                case WorldDirection.S:
                    result.Add(SpankerRight());
                    break;
                default:
                    result.Add(Down(SailType.Gaf));
                    break;
            }
            //jibs
            switch (windRelative)
            {
                case WorldDirection.N:
                case WorldDirection.NE:
                case WorldDirection.E:
                    result.Add(JibRight());
                    break;
                case WorldDirection.SE:
                case WorldDirection.S:
                    result.Add(JibLeft());
                    break;
                default:
                    result.Add(Down(SailType.FrontJib));
                    break;
            }

            return result;
        }
        
        public static IEnumerable<IShipOrder> TurnLeft(WorldDirection windRelative)
        {
            var result = new List<IShipOrder>();
            
            //spanker
            switch (windRelative)
            {
                case WorldDirection.N:
                case WorldDirection.NE:
                case WorldDirection.E:
                    result.Add(SpankerRight());
                    break;
                case WorldDirection.SE:
                case WorldDirection.S:
                    result.Add(SpankerLeft());
                    break;
                default:
                    result.Add(Down(SailType.Gaf));
                    break;
            }
            //jibs
            switch (windRelative)
            {
                case WorldDirection.N:
                case WorldDirection.NW:
                case WorldDirection.W:
                    result.Add(JibLeft());
                    break;
                case WorldDirection.SW:
                case WorldDirection.S:
                    result.Add(JibRight());
                    break;
                default:
                    result.Add(Down(SailType.FrontJib));
                    break;
            }

            return result;
        }

        private SailType _sailType;
        private int _setup;
        public int _angle;

        private SailOrder(SailType sailType, int setup, int angle = 0)
        {
            _sailType = sailType;
            _setup = setup;
            _angle = angle;
        }

        public bool Execute(ShipBody ship)
        {
            ship.SetupSail(_sailType,_setup,_angle);
            return true;
        }

        public bool Simulate(ManeuverContext context, float deltaTime)
        {
            var sail = context.Self.RigState[_sailType];
            sail.Setup = _setup;
            sail.Angle = _angle;
            context.Self.RigState[_sailType] = sail;
            return true;
        }

        public ShipOrderCategory Category => ShipOrderCategory.Sails;

        public override string ToString()
        {
            return $"[SailOrder_{_sailType}_{_setup}_{_angle}";
        }
    }
    
    
}