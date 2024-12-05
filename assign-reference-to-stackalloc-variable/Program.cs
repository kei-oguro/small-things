/* Can not compile this.
Span<string> s = stackalloc string[2] { null, null };
Console.WriteLine($"{s.Length}, {s[0]}, {s[1]}");
*/

Span<int> i = stackalloc int[2] { 42, 777 };
Console.WriteLine($"{i.Length}, {i[0]}, {i[1]}");

