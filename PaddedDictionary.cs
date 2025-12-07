namespace ObjectCompare;

/// <summary>
/// This class takes in an instance of <see cref="IDictionary{string, string}"/> and provides
/// accessors for the <see cref="IDictionary.Keys"/> and <see cref="IDictionary.Values"/> properties
/// that text forms that are padded to the maximum key/value size of the dictionary's respective
/// collections. E.g., if the max length of all keys is 10, then each normalized key returned
/// is right-padded with spaces to a length of 10. The same applies to instance's 
/// <see cref="IDictionary.Values"/> collection.
/// </summary>
/// <typeparam name="T">The instance data type to get property values for.</typeparam>
public class PaddedDictionary<T> : SortedDictionary<string, string>,
                                   IPaddedDictionary
                                   
{
    #region Constructors
    /// <summary>
    /// Default constructor.
    /// </summary>
    public PaddedDictionary()
    {
        KeyList = new List<string>();
    }

    /// <summary>
    /// Instantiates the dictionary with keys/values from the specified <paramref name="source"/>.
    /// </summary>
    /// <param name="source">The source object to get keys/values from.</param>
    public PaddedDictionary(T source)
        : this()
    {
        Initialize(source);
    }
    #endregion

    #region IPaddedDictionary Implementation
    /// <inheritdoc/>
    public Type? SourceType { get; protected set;}

    /// <inheritdoc/>
    public string SourceTypeName => SourceType!.Name;

    /// <inheritdoc/>
    public int DisplayWidth => MaxKeySize 
                                + ValueSeparatorSize 
                                + MaxValueSize;
 
    /// <inheritdoc/>
    public int MaxKeySize { get; protected set; }

    /// <inheritdoc/>
    public int MaxValueSize {get; protected set; }

    /// <inheritdoc/>
    public int ValueSeparatorSize => 1;

    /// <inheritdoc/>
    public int Length => Keys.Count;

    /// <inheritdoc/>
    public List<string> KeyList { get; protected set; }

    /// <inheritdoc/>
    public string GetHeader()
    {
        var title = SourceTypeName;
        int titleLength = title!.Length;
        if (titleLength == 0) title = string.Empty;
        if (titleLength > DisplayWidth) title = title.Substring(0, titleLength);

        var header = title.PadRight(DisplayWidth, ' ');
        return header;
    }

    /// <inheritdoc/>
    public string GetHeaderSeparator()
        => "".PadRight(DisplayWidth, '-');

    /// <inheritdoc/>
    public string GetKeyByIndex(int index)
        => KeyList[index];

    /// <inheritdoc/>
    public string GetPair(string key)
    {
        var list = KeyList;
        return $"{key} {this[key]}";
    }

    /// <inheritdoc/>
    public new string this[string key]
    {
        get
        {
            // perform a space-insensitive lookup of the key value
            var trimmedKey = key!.Trim();
            var actualKey = string.Empty;
            foreach (var testKey in Keys)
            {
                if (testKey.Trim().Equals(trimmedKey))
                {
                    // we have a match
                    actualKey = testKey;
                    break;
                }
            }

            // now perform the actual lookup
            return base[actualKey];
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// Initializes the dictionary, iterating over the collection and calculating
    /// metrics.
    /// </summary>
    /// <param name="source">The source instance to get metrics from.</param>
    public void Initialize(T source)
    {
        Clear();
        KeyList = new List<string>();
        if (source == null) return;

        // record dictionary stats
        SourceType = typeof(T);
        MaxKeySize = 0;
        MaxValueSize = 0;

        var propertyValues = ObjectHelper.GetPropertyValues(source);
        if (propertyValues.Keys.Count < 1) return;

        // determine the key and property lengths
        foreach (string key in propertyValues.Keys)
        {
            int keyLength = key!.Length;
            if (keyLength > MaxKeySize) MaxKeySize = keyLength;

            string value = propertyValues[key];
            int valueLength = value!.Length;
            if (valueLength > MaxValueSize) MaxValueSize = valueLength;
        }

        // normalize the key/property representations
        foreach (string key in propertyValues.Keys)
        {
            var keyText = Pad(key, MaxKeySize);
            string value = propertyValues[key];
            value = Pad(value, MaxValueSize);

            Add(keyText, value);
        }

        // record the list of keys
        KeyList = Keys.ToList();
    }

    public string Pad(string source, int length)
    {
        if (length < 0) throw new ArgumentException(nameof(length), $"{nameof(length)} must be greater than zero.");
        if (source == null) throw new ArgumentNullException(nameof(source));

        if (source.Length > length) source = source.Substring(0, length);

        return source.PadRight(length, ' ');        
    }
    #endregion
}