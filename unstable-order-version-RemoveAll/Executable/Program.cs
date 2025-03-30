var originalList = Enumerable.Repeat(new Random(), 20)
    .Select(rand => rand.Next() % 10)
    .ToList();
WriteLine(originalList);

MakeList(i => (i & 1) == 0/*remove even*/, "odd list");
MakeList(i => (i & 1) == 1/*remove odd*/, "even list");

void MakeList(Predicate<int> predicate, string name)
{
    var newList = new List<int>(originalList) as IList<int>;
    newList.RemoveAllUnstable(predicate);
    WriteLine(newList);
    if (newList.Any(x => predicate(x)))
    {
        Console.WriteLine($"Someghing wrong about ${name}.");
    }
}

static void WriteLine<T>(ICollection<T> collection) where T : notnull
{
    Console.WriteLine($"{collection.Count()} items: " +
        string.Join(", ", collection.Select(x => x.ToString())));
}
