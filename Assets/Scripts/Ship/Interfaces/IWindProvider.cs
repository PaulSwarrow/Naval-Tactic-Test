
using UnityEngine;

namespace Ship.Interfaces
{
    public interface IWindProvider
    {
        Vector3 GetWind(Vector3 position);
    }
}