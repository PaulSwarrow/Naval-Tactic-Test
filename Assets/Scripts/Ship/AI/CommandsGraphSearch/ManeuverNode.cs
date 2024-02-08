
using Ship.Data;

namespace Ship.AI.CommandsGraphSearch
{
    public struct ManeuverNode
    {
        public int LinearForce;
        public int AngularForce;
        public ShipConfiguration Configuration;

        public override string ToString()
        {
            return $"LF: {LinearForce}, AF: {AngularForce}, Sails: ";
        }
    }

}