using System;

namespace Ship.Utils
{
    public static class EnumTools
    {
        public static T[] ToArray<T>() where T : Enum
        {
            return (T[])Enum.GetValues(typeof(T));
        }
    }
}