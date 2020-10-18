using System;
using System.Collections;
using System.Collections.Generic;
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

        public static ObservableCollection<TResult> AsObservableCollection<T, TResult>(this IEnumerable<T> collection, Func<T, TResult> getResult)
        {
            var observableCollection = new ObservableCollection<TResult>();
            foreach (var item in collection)
            {
                var result = getResult(item);
                observableCollection.Add(result);
            }

            return observableCollection;
        }
    }
}
