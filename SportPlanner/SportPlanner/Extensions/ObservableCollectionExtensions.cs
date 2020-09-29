using System;
using System.Collections.ObjectModel;

namespace SportPlanner.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static bool ContainsValue<T>(this ObservableCollection<T> collection, T value) 
            where T : IEquatable<T>
        {
            foreach (var item in collection)
            {
                if (item.Equals(value))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
