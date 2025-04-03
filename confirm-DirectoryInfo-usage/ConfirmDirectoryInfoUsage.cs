using System.IO;

public class ConfirmDirectoryInfoUsage
{
    private const string TargetPath = @"Folder/Sub";

    public static void Main()
    {
        DirectoryInfo di = new(Path.Combine(Environment.CurrentDirectory, TargetPath));
        Try(() => di.Delete(), "Deleting non-existing(?) directory"); // can throw DirectoryNotFoundException.
        Try(() => di.Create(), "Creating a directory");
        Try(() => di.Create(), "Creating a directory already exists"); // Doesn't throw.
        Try(() => di.Delete(), "Deleting a directory");
        Try(() => di.Delete(), "Deleting exactly non-existing directory"); // Throws DirectoryNotFoundException.

        static void Try(Action action, string description)
        {
            try
            {
                action();
                Console.WriteLine($"<{description}> succeeded.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"<{description}> throws <{e.GetType().Name}>: {e.Message}");
            }
        }
    }
}