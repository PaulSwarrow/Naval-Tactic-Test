using System.Collections.Generic;
using Ship.Data;
using Ship.Interfaces;

namespace Assets.Scripts.Ship.Data.Setups
{
    /// <summary>
    /// Represents a sail type
    /// </summary>
    public class SailSpec : ISailSetup
    {

        private SailType Sail;
        public int[] Angles;
        public ActionCost TurnCost;//may depend. TODO: think of some function
        public ActionCost RaiseCost;
        public ActionCost RemoveCost;
        public bool UseForTurn; //used by AI. TODO: tag system instead?
        public int Turn;
        public int Push;
        //TODO: strafe factor? not sure


        public IReadOnlyList<int> AvailableAngles => Angles;
        public IEnumerable<ShipSailState> GetAllConfigurations()
        {
            var result = new ShipSailState[Angles.Length + 1];
            result[0] = new ShipSailState();//lowered
            for (int i = 0; i < Angles.Length; i++)
            {
                result[i + 1] = new ShipSailState()
                {
                    Setup = 1,
                    Angle = Angles[i]
                };
            }

            return result;
        }

        public bool UsedForTurns => UseForTurn;
        public double TurnFactor => Turn;
        public double PushFactor => Push;
        public SailType SailType => Sail;
    }
}