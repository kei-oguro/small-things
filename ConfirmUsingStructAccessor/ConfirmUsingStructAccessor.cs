// I wanted to know in the case we can't call a setter of a struct returned through a getter.
public class ConfirmUsingStructAccessor
{
    public struct MinimumConfirmation
    {
        public readonly int Value { get => 0; set { } }
        public readonly float Method() => 0;
        public static MinimumConfirmation Create() => new();
        public readonly MinimumConfirmation GetMethod() => this;
        public readonly MinimumConfirmation GetProperty => this;

        public static void Confirm()
        {
            // Access the value just returned.
            Create().Value = 0;
            Create().Method();
            Create().GetMethod().Value = 0;
            Create().GetMethod().Method();
            // Create().GetProperty.Value = 0; // NG
            Create().GetProperty.Method();
            Console.WriteLine(Create().Value);
            Console.WriteLine(Create().Method());
            Console.WriteLine(Create().GetMethod().Value);
            Console.WriteLine(Create().GetMethod().Method());
            Console.WriteLine(Create().GetProperty.Value);
            Console.WriteLine(Create().GetProperty.Method());

            // Access the value through a local variable that stored in the stack.
            var c = Create();
            c.Value = 0;
            var g = c.GetMethod();
            g.Value = 0;
            var p = c.GetProperty;
            p.Value = 0;
            Console.WriteLine(c.Value);
            Console.WriteLine(g.Value);
            Console.WriteLine(p.Value);
            Console.WriteLine(c.Method());
            Console.WriteLine(g.Method());
            Console.WriteLine(p.Method());

            // Access the value just returned, but owner is a variable.
            c.GetMethod().Value = 0;
            c.GetMethod().Method();
            // c.GetProperty.Value = 0; // NG
            Console.WriteLine(c.GetMethod().Value);
            Console.WriteLine(c.GetMethod().Method());
            Console.WriteLine(c.GetProperty.Value);
            Console.WriteLine(c.GetProperty.Method());
        }
    }

    public static void Main()
    {
        MinimumConfirmation.Confirm();
    }
}