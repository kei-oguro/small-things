using System.Collections;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ReflectionDictionaryJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        bool canConvert = objectType.IsAssignableTo(typeof(IDictionary));
        //Console.WriteLine($"  {canConvert} <= {objectType.Name}");
        return canConvert;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        // There are many reflections, it maybe good to cache them with keying by objectType.
        var keyValueType = typeof(KeyValuePair<,>).MakeGenericType(objectType.GenericTypeArguments);
        var list = serializer.Deserialize<IList<object>>(reader)!;
        var dictionaryConstructor = objectType.GetConstructor([])!;
        var newDictionary = dictionaryConstructor.Invoke([]);
        var adder = objectType.GetMethod("Add", objectType.GenericTypeArguments)!;
        var keyGetter = keyValueType
            .GetProperty("Key")!
            .GetAccessors()
            .First(info => info.Name == "get_Key");
        var valueGetter = keyValueType
            .GetProperty("Value")!
            .GetAccessors()
            .First(info => info.Name == "get_Value");
        foreach (var kv in list.Select(o => ((JObject)o).ToObject(keyValueType)))
        {
            adder.Invoke(newDictionary, [keyGetter.Invoke(kv, []), valueGetter.Invoke(kv, [])]);
        }
        return newDictionary;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        var dictionary = (IDictionary)value!;
        var keyValueconstructor = GetKeyValueConstructor(dictionary);
        var keys = Enumerate(dictionary.Keys);
        var values = Enumerate(dictionary.Values);
        var pairs = keys
            .Zip(values, (key, value) => keyValueconstructor.Invoke([key, value]))
            .ToList();

        serializer.Serialize(writer, pairs);

        static IEnumerable<object> Enumerate(ICollection collection)
        {
            foreach (var item in collection)
            {
                yield return item;
            }
        }
    }

    static ConstructorInfo GetKeyValueConstructor(Type dictionaryType)
    {
        var keyValueType = typeof(KeyValuePair<,>).MakeGenericType(dictionaryType.GenericTypeArguments);
        var constructorInfo = keyValueType.GetConstructor(dictionaryType.GenericTypeArguments)!;
        return constructorInfo;
    }

    static ConstructorInfo GetKeyValueConstructor(IDictionary dictionary) => GetKeyValueConstructor(dictionary.GetType());
}
