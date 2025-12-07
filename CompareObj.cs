namespace ObjectCompare;

/// <summary>
/// This outlines the details of the object under comparison.
/// </summary>
/// <typeparam name="T">The type of object to compare.</typeparam>
public class CompareObj<T>
{
    /// <summary>
    /// Instantiates the object with comparison dteails from the supplied instance.
    /// </summary>
    /// <param name="instance">The instance to get property values from.</param>
    /// <exception cref="ArgumentNullException">Throw if <paramref name="instance"/> 
    /// is <c>null</c></exception>
    public CompareObj(T instance)
    {
        if (instance == null)
        {
            throw new ArgumentNullException(nameof(instance));
        }

        DataType = instance.GetType();
        Values = ObjectHelper.GetPropertyValues(instance);
    }

    /// <summary>
    /// Gets the data type of the object under comparison.
    /// </summary>
    public Type DataType { get; protected set; }

    /// <summary>
    /// Gets the property values of the object instance under comparison.
    /// </summary>
    public IDictionary<string, string> Values { get; protected set; }
}