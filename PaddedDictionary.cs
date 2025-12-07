namespace ObjectCompare;

/// <summary>
/// This defines the properties and methods for an instance of <see cref="SortedDictionary{string, string}"/>
/// used to retrieve property names and values.
/// </summary>
public interface IPaddedDictionary
{
    /// <summary>
    /// Gets the source data type.
    /// </summary>
    Type? SourceType { get; }

    /// <summary>
    /// Gets the name of the source type.
    /// </summary>
    string SourceTypeName { get; }

    /// <summary>
    /// Gets the maximum horizontal text display size 
    /// </summary>
    int DisplayWidth { get; }

    /// <summary>
    /// Gets the maximum text key value size.
    /// </summary>
    int MaxKeySize { get; }

    /// <summary>
    /// Gets the maximum text value size.
    /// </summary>
    int MaxValueSize { get; }

    /// <summary>
    /// Gets the size in characters to separate keys/values when displayed horizontally.
    /// </summary>
    int ValueSeparatorSize { get; }

    /// <summary>
    /// Gets the header to use for the collection.
    /// </summary>
    /// <returns>The header to use for the collection.</returns>
    string GetHeader();

    /// <summary>
    /// Returns a horizontal separator for use in text display.
    /// </summary>
    /// <returns>The horizontal separator for the property names/values.</returns>
    string GetHeaderSeparator();
}

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
    #endregion

    #region Public Methods
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