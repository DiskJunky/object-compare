namespace ObjectCompare;

/// <summary>
/// This class takes in an instance of <see cref="IDictionary{string, string}"/> and provides
/// accessors for the <see cref="IDictionary.Keys"/> and <see cref="IDictionary.Values"/> properties
/// that text forms that are padded to the maximum key/value size of the dictionary's respective
/// collections. E.g., if the max length of all keys is 10, then each normalized key returned
/// is right-padded with spaces to a length of 10. The same applies to instance's 
/// <see cref="IDictionary.Values"/> collection.
/// </summary>
public class PaddedDictionary : SortedDictionary<string, string>
{
    #region Constructors
    /// <summary>
    /// Default constructor.
    /// </summary>
    public PaddedDictionary() {}

    /// <summary>
    /// Instantiates the dictionary with keys/values from the specified <paramref name="source"/>.
    /// </summary>
    /// <param name="source">The source dictionary to calculate metrics from.</param>
    public PaddedDictionary(IDictionary<string, string> source)
        : this()
    {
        Initialize(source);
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets the maximum text key value size.
    /// </summary>
    public int MaxKeySize { get; protected set; }

    /// <summary>
    /// Gets the maximum text value size.
    /// </summary>
    public int MaxValueSize {get; protected set; }
    #endregion

    #region Methods
    /// <summary>
    /// Initializes the dictionary, iterating over the collection and calculating
    /// metrics.
    /// </summary>
    /// <param name="source">The source dictionary to get metrics from.</param>
    public void Initialize(IDictionary<string, string> source)
    {
        Clear();
        MaxKeySize = 0;
        MaxValueSize = 0;
        if (source == null || source.Keys.Count < 1) return;

        foreach (string key in source.Keys)
        {
            int keyLength = key!.Length;
            if (keyLength > MaxKeySize) MaxKeySize = keyLength;

            string value = source[key];
            int valueLength = value!.Length;
            if (valueLength > MaxValueSize) MaxValueSize = valueLength;

            Add(key, value);
        }
    }
    #endregion
}