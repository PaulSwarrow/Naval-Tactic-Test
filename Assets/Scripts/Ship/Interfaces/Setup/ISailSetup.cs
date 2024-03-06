using System.Collections.Generic;
using Ship.Data;

namespace Ship.Interfaces
{
    public interface ISailSetup
    {
        IReadOnlyList<int> AvailableAngles { get; }

        IEnumerable<ShipSailState> GetAllConfigurations(); 
        bool UsedForTurns { get; }
        double TurnFactor { get; }
        double PushFactor { get; }
        SailType SailType { get; }
    }
}