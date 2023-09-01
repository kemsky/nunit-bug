namespace common.Comparators;

public static class CollectionEquality
{
    public static bool OrderedEquals<T>(ICollection<T> a, ICollection<T> b)
    {
        return new CollectionComparer<T>().Equals(a, b);
    }


    public static bool UnorderedEquals<T>(ICollection<T> a, ICollection<T> b)
    {
        if (a == null && b == null)
        {
            return true;
        }

        if (b == null || a == null)
        {
            return false;
        }

        if (a.Count != b.Count)
        {
            return false;
        }

        var d = new Dictionary<T, int>(a.Count);

        foreach (var item in a)
        {
            if (d.TryGetValue(item, out var c))
            {
                d[item] = c + 1;
            }
            else
            {
                d.Add(item, 1);
            }
        }

        foreach (var item in b)
        {
            if (d.TryGetValue(item, out var c))
            {
                if (c == 0)
                {
                    return false;
                }
                else
                {
                    var value = c - 1;

                    if (value < 0)
                    {
                        return false;
                    }

                    d[item] = value;
                }
            }
            else
            {
                return false;
            }
        }

        foreach (var v in d.Values)
        {
            if (v != 0)
            {
                return false;
            }
        }

        return true;
    }


    public static int GetHashCode<T>(ICollection<T> collection)
    {
        var comparer = new CollectionComparer<T>();

        return comparer.GetHashCode(collection);
    }
}