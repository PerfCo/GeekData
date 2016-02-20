using System.Collections.Generic;
using Nelibur.Sword.Extensions;

namespace Core.Extensions
{
    public static class ListExtenstions
    {
        public static T TakeFirst<T>(this List<T> value)
            where T : class
        {
            if (value.IsNullOrEmpty())
            {
                return default(T);
            }
            T result = value[0];
            value.RemoveAt(0);
            return result;
        }
    }
}
