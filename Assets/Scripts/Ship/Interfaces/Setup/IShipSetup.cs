using System.Collections.Generic;
using Assets.Scripts.Ship.AI.SailSchematics;
using Ship.Data;

namespace Ship.Interfaces
{
    /// <summary>
    /// Provides access for current ship setup. May take in account damaged and lost components.
    /// </summary>
    public interface IShipSetup
    {
        ISailSetup[] GetSails();
        
        int[] SailAnglesAvailable(SailType type);

        int[] SailSetupsAvailable(SailType type);
        SailType[] GetAllSails();

        IReadOnlyDictionary<SailType, SailScheme[]> GetSchemes();
    }
}