using Ship.Data;

namespace Ship.Interfaces
{
    public interface IShipSetup
    {
        int[] SailAnglesAvailable(SailType type);

        int[] SailSetupsAvailable(SailType type);
    }
}