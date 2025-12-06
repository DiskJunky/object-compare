using static TestDateTIme.ObjectHelper;
using static System.Console;

namespace TestDateTIme;

/// <summary>
/// This is the program entry point.
/// </summary>
internal class Program
{
    /// <summary>
    /// THe main starting point for the test program.
    /// </summary>
    /// <param name="args">Optional, any command line arguments supplied to the program.</param>
    public static void Main(string[] args)
    {
        WriteLine("Testing stringified object...\n");

        var testDt = DateTime.UtcNow;
        var testDto = DateTimeOffset.UtcNow;
        WriteLine($"Turning {nameof(TestObj)} into text;");

        var textBlock = Stringify(testDt).Split("\r\n");
        var dtoTextBlock = Stringify(testDto).Split("\r\n");

        // TODO:
        // * padd out the right side of 'textBlock' to longest property value before merging dtoTextBlock
        // * sort/merge/align the Datetime -> DateTimeOffset properties so we can see what's new


        // merge side by side
        var lines = new List<string>();
        for (int i = 0; i < textBlock.Length; i++)
        {
            lines.Add($"{textBlock[i]}    {dtoTextBlock[i]}");
        }

        WriteLine(string.Join("\r\n", lines));
    }
}

public class TestObj
{
    public string OneValue => "A value!";

    public string AnotherValue => "Another value is here!";

    /// <inheritdoc/>
    public override string ToString()
    {
        return Stringify(this);
    }
}