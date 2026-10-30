
using System;
using System.Collections.Generic;

public static class CollectionHelper<T>
{
    public static bool Any(
        IEnumerable<T> collection,
        Predicate<T> predicate)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        foreach (T item in collection)
        {
            if (predicate(item))
            {
                return true;
            }
        }

        return false;
    }
}
