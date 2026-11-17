
using System;
using System.Collections.Generic;

public static class CollectionUtils
{
    public static IEnumerable<TResult> Map<TSource, TResult>(
        IEnumerable<TSource> collection,
        Func<TSource, TResult> mapper)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        if (mapper == null)
            throw new ArgumentNullException(nameof(mapper));

        List<TResult> result = new List<TResult>();

        foreach (TSource item in collection)
        {
            result.Add(mapper(item));
        }

        return result;
    }

    public static T MaxBy<T, TKey>(
        IEnumerable<T> collection,
        Func<T, TKey> keySelector)
        where TKey : IComparable<TKey>
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        if (keySelector == null)
            throw new ArgumentNullException(nameof(keySelector));

        bool first = true;
        T maxItem = default(T);
        TKey maxKey = default(TKey);

        foreach (T item in collection)
        {
            TKey key = keySelector(item);

            if (first || key.CompareTo(maxKey) > 0)
            {
                maxKey = key;
                maxItem = item;
                first = false;
            }
        }

        if (first)
            throw new InvalidOperationException("Колекція порожня.");

        return maxItem;
    }

    public static IEnumerable<T> Filter<T>(
        IEnumerable<T> collection,
        Predicate<T> predicate)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        List<T> result = new List<T>();

        foreach (T item in collection)
        {
            if (predicate(item))
            {
                result.Add(item);
            }
        }

        return result;
    }

    public static void ForEach<T>(
        IEnumerable<T> collection,
        Action<T> action)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        if (action == null)
            throw new ArgumentNullException(nameof(action));

        foreach (T item in collection)
        {
            action(item);
        }
    }
}
