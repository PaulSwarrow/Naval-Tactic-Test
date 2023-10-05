using UnityEngine;

namespace Ship.Interfaces
{
    public interface IDepthProvider
    {
        float GetDepth(Vector3 position);
    }
}