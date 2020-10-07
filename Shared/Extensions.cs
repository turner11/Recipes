using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public static class Extensions
{

    public static bool GuessIsRtl(this string str)
    {
        var cleanChars = str.Trim().Replace(" ", "").Where(c => !char.IsPunctuation(c)).ToArray();
        var avg = cleanChars.Select(c => (int)c).Average();
        var isRtl = 'א' < avg && avg < 'ת';
        return isRtl;
    }
    public static string GuessDirection(this string str)
    {
        return str.GuessIsRtl() ? "rtl" : "ltr";
    }

    public static string GuessRtlClass(this string str)
    {
        return str.GuessIsRtl() ? "rtl_class" : "";

    }

    public static IEnumerable<TSource> DistinctBy<TSource, TKey>
    (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        HashSet<TKey> seenKeys = new HashSet<TKey>();
        foreach (TSource element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }
}