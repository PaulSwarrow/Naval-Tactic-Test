using System;
using System.Collections.Generic;

namespace Ship.AI.Data
{
    [Serializable]
    public class ManeuverPrediction
    {
        public List<ManeuverPredictionPhase> Trajectory = new List<ManeuverPredictionPhase>();

    }

    [Serializable]
    public struct ManeuverPredictionPhase
    {
        public AIPredictionShipData Self;
        public float Timestamp;
        public IShipOrder[] Orders { get; set; }
    }
}