using System.Collections;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace ObjectCompare;

/// <summary>
/// THis class contains helper methods that apply across all objects.
/// </summary>
public static class ObjectHelper
{
    /// <summary>
    /// This gets the property names and associated values at a shallow
    /// level of the object. Collections are retrieved with their collection
    /// count and any <c>null</c> are reported back as such in text form. If
    /// the object/value doesn't have any property values, then an empty
    /// <see cref="IDictionary"/> instance is returned.
    /// </summary>
    /// <param name="o">The object to get the property values for.</param>
    /// <typeparam name="T">The type of object to to get the property values for.</typeparam>
    /// <returns>The property values of the  object.</returns>
    public static IDictionary<string, string> GetPropertyValues<T>(T o)
    {
        const string NULL_TEXT = "<null>";
        IDictionary<string, string> propValues = new SortedDictionary<string, string>();

        if (o == null) return propValues;

        // we have a value, see if it's
        var type = typeof(T);
        var decimalType = typeof(decimal);
        var isDateType = typeof(DateTime).Equals(type) 
                            || typeof(DateTimeOffset).Equals(type);
        var isStruct = type.IsValueType
                        && !type.IsEnum
                        && !type.IsPrimitive
                        && type != decimalType;
        var enumerableType = typeof(IEnumerable);
        var stringType = typeof(string);
        if (type == stringType
            || (type.IsValueType && !isStruct && !isDateType)
            || enumerableType.IsAssignableFrom(type))
        {
            // we just have a vanilla value type
            propValues.Add(string.Empty, o.ToString() ?? NULL_TEXT);
            return propValues;
        }

        // we have an object or struct. A struct is a special case as it's a value type but has
        // properties that can be iterated over but there's no direct "Is" property on 'type' that
        // allows us to directly test this so we use "is value type but isn't an enum" to check.
        MemberInfo[] members;
        if (!type.IsValueType || isDateType)
        {
            // get the properties
            members = type.GetProperties()
                            .Cast<MemberInfo>()
                            .ToArray();
        }
        else
        {
            // get the struct's fields
            members = type.GetFields(BindingFlags.Instance | BindingFlags.Public)
                            .Cast<MemberInfo>()
                            .ToArray();
        }

        // build the property collection
        foreach (var m in members)
        {
            // what property/field type are we dealing with
            var memberType = isStruct && !isDateType ? ((FieldInfo)m).FieldType : ((PropertyInfo)m).PropertyType;
            object? memberValue = null;
            if (isStruct && !isDateType)
            {
                // get the field value
                memberValue = ((FieldInfo)m).GetValue(o);
            }
            else if ((enumerableType.IsAssignableFrom(memberType)
                        && !stringType.Equals(memberType))
                        || ((PropertyInfo)m).GetIndexParameters().Length > 0)
            {
                // skip over enumerable/indexer properties
                continue;
            }
            else
            {
                // get the property value
                memberValue = ((PropertyInfo)m).GetValue(o, null);
            }

            // are we dealing with an numerable that isn't a string
            if (enumerableType.IsAssignableFrom(memberType)
                && !stringType.Equals(memberType))
            {
                // is the enumerable is null
                if (memberValue == null)
                {
                    propValues.Add(m.Name, NULL_TEXT);
                }
                else
                {
                    // get the enumerable count
                    var enumerable = (IEnumerable)memberValue;
                    var count = 0;
                    foreach (object _ in enumerable)
                    {
                        count++;
                    }

                    propValues.Add(m.Name, $"<collection[{count}]>");
                }
            }
            else
            {
                // get the property value
                if (memberValue == null)
                {
                    propValues.Add(m.Name, NULL_TEXT);
                }
                else
                {
                    propValues.Add(m.Name, $"{memberValue}");
                }
            }
        }

        return propValues;
    }

}
