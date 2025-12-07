using System.Text;
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
        var newLine = Environment.NewLine;
        WriteLine($"Testing stringified objects...{newLine}");

        var testDt = DateTime.UtcNow;
        var testDto = DateTimeOffset.UtcNow;
        WriteLine($"Turning {nameof(DateTime)} into text;");

        var dtValues = new CompareObj<DateTime>(testDt);
        var dtoValues = new CompareObj<DateTimeOffset>(testDto);

        // TODO:
        // * padd out the right side of 'textBlock' to longest property value before merging dtoTextBlock
        // * sort/merge/align the Datetime -> DateTimeOffset properties so we can see what's new


        // merge side by side
        // var lines = new List<string>();
        // for (int i = 0; i < textBlock.Length; i++)
        // {
        //     lines.Add($"{textBlock[i]}    {dtoTextBlock[i]}");
        // }

        // WriteLine(string.Join($"{newLine}", lines));
    }

    private static string DictToString(Dictionary<string, string> dict)
    {
        if (dict == null || dict.Keys.Count < 1) return string.Empty;

        // calculate the longest key
        var sb = new StringBuilder();
        var padLength = dict.Keys.Max(k => k?.Length) ?? 0;
        var keys = dict.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            sb.AppendLine($"{keys[i].PadRight(padLength, ' ')}: {dict[keys[i]]}");
            //sb.AppendLine($"{keys[i].PadRight(padLength, ' ')}: {dict[keys[i]]}");
        }

        return sb.ToString();
    }
}

/// <summary>
/// This outlines the details of the object under comparison.
/// </summary>
/// <typeparam name="T">The type of object to compare.</typeparam>
public class CompareObj<T>
{
    /// <summary>
    /// Instantiates the object with comparison dteails from the supplied instance.
    /// </summary>
    /// <param name="instance">The instance to get property values from.</param>
    /// <exception cref="ArgumentNullException">Throw if <paramref name="instance"/> 
    /// is <c>null</c></exception>
    public CompareObj(T instance)
    {
        if (instance == null)
        {
            throw new ArgumentNullException(nameof(instance));
        }

        DataType = instance.GetType();
        Values = GetPropertyValues(instance);
    }

    /// <summary>
    /// Gets the data type of the object under comparison.
    /// </summary>
    public Type DataType { get; protected set; }

    /// <summary>
    /// Gets the property values of the object instance under comparison.
    /// </summary>
    public IDictionary<string, string> Values { get; protected set; }
}