using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Program
{
    public class MyComplexClass(string _s, int _i)
    {
        public string s = _s;
        public int i = _i;
        public static bool Comparer(MyComplexClass x, MyComplexClass y) => x.s == y.s && x.i == y.i;
    }

    [JsonConverter(typeof(GenericsDictionaryJsonConverter<MyComplexClass, int>))]
    public readonly IReadOnlyDictionary<MyComplexClass, int> dictionary = new Dictionary<MyComplexClass, int>() { { new MyComplexClass("33", 3), 333 }, { new MyComplexClass("44", 4), 444 }, };

    public class MyClassWithAttributedDictionary
    {
        [JsonConverter(typeof(GenericsDictionaryJsonConverter<MyComplexClass, int>))]
        public IReadOnlyDictionary<MyComplexClass, int> dictionary = new Dictionary<MyComplexClass, int>() { { new MyComplexClass("55", 5), 555 }, { new MyComplexClass("66", 6), 666 }, };

        public static bool Comparer(MyClassWithAttributedDictionary x, MyClassWithAttributedDictionary y)
        {
            return x == y || Comparer(x.dictionary, y.dictionary);

            static bool Comparer(IReadOnlyDictionary<MyComplexClass, int> x, IReadOnlyDictionary<MyComplexClass, int> y)
            {
                return x.Count == y.Count && x.Zip(y).All(
                    pair => MyComplexClass.Comparer(pair.First.Key, pair.Second.Key) && pair.First.Value == pair.Second.Value);
            }
        }
    }

    private void Do()
    {
        var plainSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        var reflectionSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
            Converters = [new ReflectionDictionaryJsonConverter()]
        };

        CheckCanSerialize(new MyComplexClass("11", 1), reflectionSettings, MyComplexClass.Comparer);

        try
        {
            CheckCanSerialize(
                new Dictionary<MyComplexClass, int> {
                    { new MyComplexClass("2", 22), 222 }, { new MyComplexClass("3", 33), 333 } },
                plainSettings,
                (x, y) => x.Count == y.Count && x.Zip(y).All(
                    pair => MyComplexClass.Comparer(pair.First.Key, pair.Second.Key) && pair.First.Value == pair.Second.Value));
        }
        catch (JsonSerializationException)
        {
            Console.WriteLine("This is the problem what we want to solve.");
        }

        CheckCanSerialize(
            new Dictionary<MyComplexClass, int> {
                { new MyComplexClass("2", 22), 222 }, { new MyComplexClass("3", 33), 333 } },
            reflectionSettings,
            (x, y) => x.Count() == y.Count() && x.Zip(y).All(
                pair => MyComplexClass.Comparer(pair.First.Key, pair.Second.Key) && pair.First.Value == pair.Second.Value));

        try
        {
            CheckCanSerialize(dictionary, plainSettings,
                (x, y) => x.Count == y.Count && x.Zip(y).All(
                    pair => MyComplexClass.Comparer(pair.First.Key, pair.Second.Key) && pair.First.Value == pair.Second.Value));
        }
        catch (JsonSerializationException)
        {
            Console.WriteLine("We know it cannot be serializable. Because the serializer cannot know the dictionary has the JSonConveter attribute.");
        }

        CheckCanSerialize(
            new MyClassWithAttributedDictionary(),
            plainSettings,
            MyClassWithAttributedDictionary.Comparer);

        static bool CheckCanSerialize<T>(T value, JsonSerializerSettings settings, Func<T, T, bool> comparer)
        {
            var json = JsonConvert.SerializeObject(value, settings);
            Console.WriteLine(json);
            var deserialized = JsonConvert.DeserializeObject<T>(json, settings) ?? throw new JsonSerializationException();
            var equality = comparer(value, deserialized);
            Console.WriteLine(equality);
            return equality;
        }
    }

    internal static void Main()
    {
        (new Program()).Do();
    }

}