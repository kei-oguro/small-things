using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public static class SpanUsage
{
    private static void Main()
    {
        {
            var array = new[] { 1, 2, 3 };
            var span = new ReadOnlySpan<int>(array);
            Console.WriteLine(Sum(span));
        }

        {
            ReadOnlySpan<int> span = stackalloc int[3] { 4, 5, 6 };
            Console.WriteLine(Sum(span));
        }

        // Unity doesn't yet support collection expression.
        {
            ReadOnlySpan<int> span = [10, 11, 12];
            Console.WriteLine(Sum(span));
        }

        // Unity doesn't have CollectionsMarshal.AsSpan().
        {
            var list = new List<int> { 13, 14, 15 };
            ReadOnlySpan<int> span = CollectionsMarshal.AsSpan(list);
            Console.WriteLine(Sum(span));
        }
    }

    private static int Sum(ReadOnlySpan<int> span)
    {
        int sum = 0;
        foreach (var value in span)
        {
            sum += value;
        }
        return sum;
    }
}
