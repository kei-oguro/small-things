using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

public class GenericsDictionaryJsonConverter<TKey, TValue> : JsonConverter
    where TKey : notnull
{
    public override bool CanConvert(Type objectType)
    {
        bool canConvert = objectType.IsAssignableTo(typeof(IDictionary));
        //Console.WriteLine($"  {canConvert} <= {objectType.Name}");
        return canConvert;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        //Console.WriteLine($">>> {objectType.Name}");
        var pairs = serializer.Deserialize<IReadOnlyList<KeyValuePair<TKey, TValue>>>(reader)!;
        return pairs.ToDictionary();
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        var dictionary = value! as IDictionary<TKey, TValue> ?? throw new ArgumentException($"{value?.GetType().Name ?? ("null")} is not a {nameof(IDictionary)}<{nameof(TKey)}, {nameof(TValue)}");
        // We can write a dictionary to list convertion as `[.. dictionary]` instead of `dictionary.ToList()`. But it will be serialized as `<>z__ReadOnlyArray`1[[],[], System.Private.CoreLib]]. We don't have a way to get back it.
        IReadOnlyList<KeyValuePair<TKey, TValue>> pairs = dictionary.ToList();
        serializer.Serialize(writer, pairs);
    }
}
