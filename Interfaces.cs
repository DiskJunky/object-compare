
namespace ObjectCompare;

/// <summary>
/// This defines the properties and methods for an instance of <see cref="SortedDictionary{string, string}"/>
/// used to retrieve property names and values.
/// </summary>
internal interface IPaddedDictionary
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
    /// Gets the size of the collection.
    /// </summary>
    int Length { get; }

    /// <summary>
    /// Gets the key collection.
    /// </summary>
    SortedDictionary<string, string>.KeyCollection Keys { get; }

    /// <summary>
    /// Gets the value for the specified key.
    /// </summary>
    /// <param name="key">The key to get a value for.</param>
    /// <returns>The value for the specified key.</returns>
    string this[string key] { get; }

    /// <summary>
    /// Gets or sets the list of keys, listed by index, as they appear in base dictionary.
    /// </summary>
    List<string> KeyList { get; }


    /// <summary>
    /// Returns if the specified key exists in the collection.
    /// </summary>
    /// <param name="key">The key to lookup.</param>
    /// <returns><c>true</c> if the key exists in the collection <c>false</c> otherwise.</returns>
    bool ContainsKey(string key);

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

    /// <summary>
    /// Return the key at the specified index in the collection.
    /// </summary>
    /// <param name="index">The index to get the key from.</param>
    /// <returns>The key at the specified index.</returns>
    string GetKeyByIndex(int index);

    string GetPair(string key);
}
