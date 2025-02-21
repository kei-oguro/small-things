var x = 0;
Console.WriteLine(x is < -1 or >= 1);
// Console.WriteLine(x is < -1 and >= 1); // Compiler report this as error because an integer never match this pattern. That's inteligent.
Console.WriteLine(x is < 2 and >= 1);
Console.WriteLine($"{x}" is "cat" or "dog");
