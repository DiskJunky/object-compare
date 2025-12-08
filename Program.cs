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
    /// The main starting point for the test program.
    /// </summary>
    /// <param name="args">Optional, any command line arguments supplied to the program.</param>
    public static void Main(string[] args)
    {
        var testDt = DateTime.UtcNow;
        var testDto = DateTimeOffset.UtcNow;
        IPaddedDictionary dtTextValues = new PaddedDictionary<DateTime>(testDt);
        IPaddedDictionary dtoTextValues = new PaddedDictionary<DateTimeOffset>(testDto);

        Compare(dtTextValues, dtoTextValues);
    }

    /// <summary>
    /// This compares the two property value collections.
    /// </summary>
    /// <param name="leftTextValues">The left property value collection.</param>
    /// <param name="rightTextValues">The right property value collection.</param>
    /// <param name="write">The function to write out to. Defaults to writing 
    /// to <see cref="Console.WriteLine(string)"/></param>
    private static void Compare(IPaddedDictionary leftTextValues, 
                                IPaddedDictionary rightTextValues,
                                Action<string>? write = null)
    {
        // if we've no writer, write out to the console by default
        if (write == null) write = WriteLine;

        var newLine = Environment.NewLine;
        write($"Comparing {leftTextValues.SourceTypeName} and {rightTextValues.SourceTypeName} objects...{newLine}");

        // create a local shorthand function that calls the same method on
        // both dictionaries
        void WriteBoth(Func<IPaddedDictionary, string> get)
        {
            var left = get(leftTextValues);
            var right = get(rightTextValues);
            WritePair(left, right, write);
        }

        // display headers
        WriteBoth(g => g.GetHeader());
        WriteBoth(g => g.GetHeaderSeparator());
        

        // merge each property side by side, with properties the same name on the same line
        int leftKeyIndex = 0;
        int rightKeyIndex = 0;
        int mainIndex = 0;
        int totalSize = leftTextValues.Length > rightTextValues.Length 
                            ? leftTextValues.Length 
                            : rightTextValues.Length;
        while (mainIndex < totalSize)
        {
            // get the property names at the current, respective index
            var leftKey = leftKeyIndex < leftTextValues.Length 
                            ? leftTextValues.KeyList[leftKeyIndex] : string.Empty;
            var rightKey = rightKeyIndex < rightTextValues.Length 
                            ? rightTextValues.KeyList[rightKeyIndex] : string.Empty;

            // are they the same
            var leftTrimmedKey = leftKey.Trim();
            var rightTrimmedKey = rightKey.Trim();
            var compareResult = String.Compare(leftTrimmedKey, rightTrimmedKey, StringComparison.InvariantCulture);
            switch (compareResult)
            {
                case 0:     // equal
                    WriteBoth(g => g.GetPair(leftKey));
                    leftKeyIndex++;
                    rightKeyIndex++;
                    break;

                case 1:     // right takes precedence
                    WritePair(PadSpace(leftTextValues.DisplayWidth), 
                              rightTextValues.GetPair(rightKey),
                              write);
                    rightKeyIndex++;
                    break;

                case -1:    // left takes precedence
                    WritePair(leftTextValues.GetPair(leftKey), 
                              PadSpace(rightTextValues.DisplayWidth),
                              write);
                    leftKeyIndex++;
                    break;
            }
            
            mainIndex++;
        }
    }

    /// <summary>
    /// This returns a set of spaces to the specified <paramref name="length"/>.
    /// </summary>
    /// <param name="length">The length of spaces to return.</param>
    /// <returns>The generated set of spaces.</returns>
    private static string PadSpace(int length)
        => "".PadRight(length);

    /// <summary>
    /// Writes the left and right text components, separated by a defined space width.
    /// </summary>
    /// <param name="left">The left text.</param>
    /// <param name="right">The right text.</param>
    /// <param name="write">The function/action to use to write out the string.</param>
    private static void WritePair(string left, string right, Action<string> write)
        => write(left + "".PadRight(SEP_SIZE) + right);
}
