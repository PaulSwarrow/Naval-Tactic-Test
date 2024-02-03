namespace Ship.AI.CommandsGraphSearch
{
    public struct ManeuverEdge
    {
        //in seconds?
        public int Cost;
        public ManeuverNode Destination;
    }
}