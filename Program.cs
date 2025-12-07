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
    /// The character width gap between the compared object property/value pairs.
    /// </summary>
    private const int SEP_SIZE = 4;

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
        // * pad out the right side of 'textBlock' to longest property value before merging dtoTextBlock
        // * sort/merge/align the Datetime -> DateTimeOffset properties so we can see what's new


        // create a local short-hand function that calls the same method on
        // both dictionaries
        Action<Func<IPaddedDictionary, string>> writeBoth = get => WritePair(get(dtTextValues), 
                                                                             get(dtoTextValues));
        
        // display headers
        writeBoth(g => g.GetHeader());
        writeBoth(g => g.GetHeaderSeparator());
        

        // merge each property side by side, with properties the same name on the same line
        int leftKeyIndex = 0;
        int rightKeyIndex = 0;
        int mainIndex = 0;
        int totalSize = dtTextValues.Length > dtoTextValues.Length 
                            ? dtTextValues.Length 
                            : dtoTextValues.Length;
        while (mainIndex < totalSize)
        {
            // get the property names at the overall index
            var leftKey = mainIndex < dtTextValues.Length ? dtTextValues.KeyList[mainIndex] : "";
            var rightKey = mainIndex < dtoTextValues.Length ? dtoTextValues.KeyList[mainIndex] : "";

            // are they the same
            if (leftKey.Equals(rightKey))
            {
                writeBoth(g => g.GetPair(leftKey));
            }
            else
            {
                // what do we do here...?

            }
            
            mainIndex++;
        }

    }

    /// <summary>
    /// Writes the left and right text components, separated by a defined space width.
    /// </summary>
    /// <param name="left">The left text.</param>
    /// <param name="right">The right text.</param>
    private static void WritePair(string left, string right)
        => WriteLine(left + "".PadRight(SEP_SIZE) + right);
}
