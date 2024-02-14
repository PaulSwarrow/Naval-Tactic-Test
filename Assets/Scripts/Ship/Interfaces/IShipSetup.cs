using System.Collections.Generic;
using Ship.AI.SailSchemantics;
using Ship.Data;

namespace Ship.Interfaces
{
    public interface IShipSetup
    {
        int[] SailAnglesAvailable(SailType type);

        int[] SailSetupsAvailable(SailType type);
        SailType[] GetAllSails();

        IReadOnlyDictionary<SailType, SailScheme[]> GetSchemes();
    }
}