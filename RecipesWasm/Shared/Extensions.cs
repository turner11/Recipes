using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


public static class Extensions
{
    public static string GetHtmlInnerText(this string html)
    {
        var doc = new HtmlAgilityPack.HtmlDocument();        
        doc.LoadHtml(html);
        
        var ns = doc.DocumentNode.SelectNodes("/").ToList();
        var txt = ns.First().InnerText;
        return txt;
    }
    public static bool GuessIsRtl(this string str)
    {
        if (String.IsNullOrWhiteSpace(str))
            return false;
        var cleanChars = str.Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "").Where(c => !char.IsPunctuation(c) && !char.IsDigit(c)).ToArray();
        var sortedInts = cleanChars.Select(c => (int)c).OrderBy(c => c).ToArray();

        var median = sortedInts[(int)(sortedInts.Length / 2)];
        //var avg = cleanChars.Select(c => (int)c).Average();
        var isRtl = 'א' <= median && median <= 'ת';
        return isRtl;
    }
    public static string GuessDirection(this string str)
    {
        return str.GuessIsRtl() ? "rtl" : "ltr";
    }

    public static string GuessDirectionStyle(this string str)
    {
        return str.GuessIsRtl() ? "direction: rtl; text-align: start;" : "";

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