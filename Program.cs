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
        WriteLine($"Comparing DateTime and DateTimeOffset objects...{newLine}");

        var testDt = DateTime.UtcNow;
        var testDto = DateTimeOffset.UtcNow;
        WriteLine($"Turning {nameof(DateTime)} into text;");


        IPaddedDictionary dtTextValues = new PaddedDictionary<DateTime>(testDt);
        IPaddedDictionary dtoTextValues = new PaddedDictionary<DateTimeOffset>(testDto);

        // create a local short-hand function that calls the same method on
        // both dictionaries
        Action<Func<IPaddedDictionary, string>> writeBoth = get => {
            var left = get(dtTextValues);
            var right = get(dtoTextValues);
            WritePair(left, right);
        };
        
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
            var leftKey = leftKeyIndex < dtTextValues.Length ? dtTextValues.KeyList[leftKeyIndex] : "";
            var rightKey = rightKeyIndex < dtoTextValues.Length ? dtoTextValues.KeyList[rightKeyIndex] : "";

            // are they the same
            var leftTrimmedKey = leftKey.Trim();
            var rightTrimmedKey = rightKey.Trim();
            var compareResult = leftTrimmedKey.CompareTo(rightTrimmedKey);
            switch (compareResult)
            {
                case 0:     // equal
                    writeBoth(g => g.GetPair(leftKey));
                    leftKeyIndex++;
                    rightKeyIndex++;
                    break;

                case 1:     // right takes precedence
                    WritePair(" ".PadRight(dtTextValues.DisplayWidth), dtoTextValues.GetPair(rightKey));
                    rightKeyIndex++;
                    break;

                case -1:    // left takes precedence
                    WritePair(dtTextValues.GetPair(leftKey), " ".PadRight(dtoTextValues.DisplayWidth));
                    leftKeyIndex++;
                    break;
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
