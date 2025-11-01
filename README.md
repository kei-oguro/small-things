# small-things

- git-merge-into-name-usage .. shows that `merge --into-name` changes the COMMIT_MSG that will be shown in the editor when merging.
- git-size-of-many-small-commits .. Shell script to see the size of a git repository with many **small commits**.
- git-size-of-many-small-files .. Shell script to see the size of a git repository with many **small files in a commit**.
- Newtonsoft.Json
  - DictionaryJsonConverter is a small project to try out the implementation of JsonConverter. In C#, we can use `Dictionary<TKey, TValue>` keyed by any class. But the name of the JSON's name-value-pair is always a string. To restore the key, it must be represented with all the necessary information, but that's difficult if we serialize the key as a property name. So I tried to write a JsonConverter that can convert dictionaries with any type as key.
    - GenericsDictionaryJsonConverter uses generics. To use this, you write like as `GenericDictionaryJsonConverter<MyClass, int>`. This is good when adding it as an attribute.
    - ReflectionDictionaryJsonConverter uses reflection. This is good if you want to serialize the all kinds of `Dictionary` without specifying anything per member. You will get it by registering this converter with the serializer.
- try-using-gpt-oss-with-llama.cpp .. Create a WSL image, install the build tools and dependencies, build llama.cpp with CUDA, quantize the gpt-oss-20B model, try some parameter combinations for performance.

<!-- cSpell:words CUDA -->
