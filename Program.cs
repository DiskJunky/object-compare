using System.Text;
using static ObjectCompare.ObjectHelper;
using static System.Console;

namespace ObjectCompare;

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


        IPaddedDictionary dtTextValues = new PaddedDictionary<DateTime>(testDt);
        IPaddedDictionary dtoTextValues = new PaddedDictionary<DateTimeOffset>(testDto);
        // TODO:
        // * padd out the right side of 'textBlock' to longest property value before merging dtoTextBlock
        // * sort/merge/align the Datetime -> DateTimeOffset properties so we can see what's new


        // display headers
        //Func<IPaddedDictionary, string> write = p => WritePair(dtTextValues)
        WritePair(dtTextValues, dtoTextValues, d => d.GetHeader());
        WritePair(dtTextValues.GetHeaderSeparator(), dtoTextValues.GetHeaderSeparator());

        // merge side by side
        // var lines = new List<string>();
        // for (int i = 0; i < textBlock.Length; i++)
        // {
        //     lines.Add($"{textBlock[i]}    {dtoTextBlock[i]}");
        // }

        // WriteLine(string.Join($"{newLine}", lines));
    }

    /// <summary>
    /// Writes the left and right text components, separated by a defined space width.
    /// </summary>
    /// <param name="left">The left text.</param>
    /// <param name="right">The right text.</param>
    private static void WritePair(string left, string right)
        => WriteLine(left + "".PadRight(4) + right);

    private static void WritePair(IPaddedDictionary left, 
                                  IPaddedDictionary right,
                                  Func<IPaddedDictionary, string> get)
        => WritePair(get(left), get(right));

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
