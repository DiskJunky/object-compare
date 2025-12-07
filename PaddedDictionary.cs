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
        var actualKey = GetActualKey(key);
        return $"{actualKey} {base[actualKey]}";
    }

    /// <inheritdoc/>
    public new string this[string key]
    {
        get => base[GetActualKey(key)];
    }
    #endregion

    #region Methods
    /// <summary>
    /// This performs a space-insensitsive search of the key name in the collection
    /// and returns the actual full key, if found, <see cref="string.Empty"/> otherwise.
    /// </summary>
    /// <param name="key">The key to look up.</param>
    /// <returns>The actual key if found, <see cref="string.Empty"/> otherwise.</returns>
    private string GetActualKey(string key)
    {
        var trimmedKey = key!.Trim();
        foreach (var testKey in Keys)
        {
            if (testKey.Trim().Equals(trimmedKey))
            {
                // we have a match
                return testKey;
            }
        }

        return string.Empty;
    }

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

    /// <summary>
    /// Pad the supplied text out to the specified <paramref name="length"/> with
    /// spaces.
    /// </summary>
    /// <param name="source">The text to pad.</param>
    /// <param name="length">The length to pad to.</param>
    /// <returns>The space-padded text.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="length"/> is 
    /// less than zero.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> 
    /// is <c>null</c>.</exception>
    public string Pad(string source, int length)
    {
        if (length < 0) throw new ArgumentException(nameof(length), $"{nameof(length)} must be greater than zero.");
        if (source == null) throw new ArgumentNullException(nameof(source));

        if (source.Length > length) source = source.Substring(0, length);

        return source.PadRight(length, ' ');        
    }
    #endregion
}