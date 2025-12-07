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
public class PaddedDictionary<T> : SortedDictionary<string, string>
{
    #region Constructors
    /// <summary>
    /// Default constructor.
    /// </summary>
    public PaddedDictionary() {}

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

    #region Properties
    /// <summary>
    /// Gets the source data type.
    /// </summary>
    public Type? SourceType { get; protected set;}

    /// <summary>
    /// Gets the name of the source type.
    /// </summary>
    public string SourceTypeName => SourceType!.Name;

    /// <summary>
    /// Gets the maximum horizontal text display size 
    /// </summary>
    public int DisplayWidth => MaxKeySize 
                                + ValueSeparatorSize 
                                + MaxValueSize;
 
    /// <summary>
    /// Gets the maximum text key value size.
    /// </summary>
    public int MaxKeySize { get; protected set; }

    /// <summary>
    /// Gets the maximum text value size.
    /// </summary>
    public int MaxValueSize {get; protected set; }

    /// <summary>
    /// Gets the size in characters to separate keys/values when displayed horizontally.
    /// </summary>
    public int ValueSeparatorSize = 1;

    #endregion

    #region Methods
    /// <summary>
    /// Gets the header to use for the collection.
    /// </summary>
    /// <returns>The header to use for the collection.</returns>
    public string GetHeader()
    {
        var title = SourceTypeName;
        int titleLength = title!.Length;
        if (titleLength == 0) title = string.Empty;
        if (titleLength > DisplayWidth) title = title.Substring(0, titleLength);

        var header = title.PadRight(DisplayWidth, ' ');
        return header;
    }

    /// <summary>
    /// Returns a horizontal separator for use in text display.
    /// </summary>
    /// <returns>The horizontal separator for the property names/values.</returns>
    public string GetHeaderSeparator()
        => "".PadRight(DisplayWidth, '-');

    /// <summary>
    /// Initializes the dictionary, iterating over the collection and calculating
    /// metrics.
    /// </summary>
    /// <param name="source">The source instance to get metrics from.</param>
    public void Initialize(T source)
    {
        Clear();
        if (source == null) return;
        SourceType = typeof(T);
        
        MaxKeySize = 0;
        MaxValueSize = 0;

        var propertyValues = ObjectHelper.GetPropertyValues(source);
        if (propertyValues.Keys.Count < 1) return;

        foreach (string key in propertyValues.Keys)
        {
            int keyLength = key!.Length;
            if (keyLength > MaxKeySize) MaxKeySize = keyLength;

            string value = propertyValues[key];
            int valueLength = value!.Length;
            if (valueLength > MaxValueSize) MaxValueSize = valueLength;

            Add(key, value);
        }
    }
    #endregion
}