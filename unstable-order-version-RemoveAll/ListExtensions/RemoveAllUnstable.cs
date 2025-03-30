public static class ListExtensions
{
    /// <summary>
    /// This is the unstable order version of the List.RemoveAll().
    ///  <para>
    ///  When it removes an item ...<list type="number">
    ///    <item>From the end of the list, it looks for an item that is not being removed.</item>
    ///    <item>It copies and overwrites the item to be removed with the item found in the previous step.</item>
    ///    <item>It shortens the length of the target list. If there are any last items to be removed, the length to be shortened is more than one.</item></list>
    ///  Such a method means that the last items will be inserted somewhere in the middle of the list. So this method doesn't preserve the order of the list during remove all. This is the meaning of the word "unstable order".
    ///  </para>
    /// </summary>
    /// <typeparam name="T">The item type of the target <see cref="IList{T}"/></typeparam>
    /// <param name="list">The target list</param>
    /// <param name="predicate">The delegate that determines whether an item is is removed</param>
    public static void RemoveAllUnstable<T>(this List<T> list, Predicate<T> predicate)
    {
        for (var i = 0; i < list.Count; ++i)
        {
            if (predicate(list[i])) // Should this one be removed?
            {
                var count = list.Count;
                int last = count - 1;
                for (; i < last; --last)
                {
                    var item = list[last];
                    if (!predicate(item)) // Should this one be kept?
                    {
                        if (i < last)
                        {
                            list[i] = item;
                        }
                        break;
                    }
                }
                list.RemoveRange(last, count - last);
            }
        }
    }

    /// <inheritdoc cref="ListExtensions.RemoveAllUnstable{T}(System.Collection.Generic.List{T}, System.Predicate{T})"/>
    /// <remarks>
    /// If there was a <c><see cref="IList{T}"/>.RemoveRange()</c>, I wanted to use it to remove all sequential, instead of calling <see cref="IList{T}.RemoveAt()"/> each time.
    /// </remarks>
    public static void RemoveAllUnstable<T>(this IList<T> list, Predicate<T> predicate)
    {
        for (var i = 0; i < list.Count; ++i)
        {
            if (predicate(list[i])) // Should this one be removed?
            {
                int last = list.Count - 1;
                for (; i < last; --last)
                {
                    var item = list[last]; // Preserve the item before remove.
                    list.RemoveAt(last);
                    if (!predicate(item)) // Should this one be kept?
                    {
                        if (i < last)
                        {
                            list[i] = item;
                        }
                        break;
                    }
                }
                if (i == last)
                {
                    list.RemoveAt(i);
                    return;
                }
            }
        }
    }
}