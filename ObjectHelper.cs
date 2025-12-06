#region FILE HEADER
/*
 *      Copyright (c) 2025 Becton Dickinson Corporation.
 *
 *  All rights reserved. Becton Dickinson source code is an unpublished
 *  work and the use of a copyright notice does not imply otherwise.
 *  This source code contains confidential, trade secret material
 *  of Becton Dickinson. Any attempt or participation in deciphering,
 *  decoding, reverse engineering or in any way altering the source
 *  code is strictly prohibited, unless the prior written consent of
 *  Becton Dickinson is obtained.  This is proprietary and confidential
 *  to Becton Dickinson.
 */
#endregion

using System.Collections;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace TestDateTIme
{
    /// <summary>
    /// THis class contains helper methods that apply across all objects.
    /// </summary>
    public static class ObjectHelper
    {
        /// <summary>
        /// This stringifies a given object by iterating over its top-level properties and producing a
        /// human-readable single line string. Collections are represented by their name and number of items.
        /// Other properties are represented by their name and a call to "ToString()" on their value (if not null).
        /// Any objects of value type (assuming non-structs) are written out with just their value.
        /// </summary>
        /// <param name="o">The object to stringify.</param>
        /// <typeparam name="T">The type of object to stringify.</typeparam>
        /// <returns>The stringified object.</returns>
        public static string Stringify<T>(T o)
        {
            if (o == null) return "null";

            // we have a value, see if it's
            var type = typeof(T);
            var memberDescriptions = new Dictionary<string, string>();
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
                return o.ToString();
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

            // build the string
            foreach (var m in members)
            {
                var memberType = isStruct && !isDateType ? ((FieldInfo)m).FieldType : ((PropertyInfo)m).PropertyType;
                object memberValue = null;
                if (isStruct && !isDateType)
                {
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
                    memberValue = ((PropertyInfo)m).GetValue(o, null);
                }

                if (enumerableType.IsAssignableFrom(memberType)
                    && !stringType.Equals(memberType))
                {
                    if (memberValue == null)
                    {
                        memberDescriptions.Add(m.Name, $"null");
                    }
                    else
                    {
                        var enumerable = (IEnumerable)memberValue;
                        var count = 0;
                        foreach (object _ in enumerable)
                        {
                            count++;
                        }

                        memberDescriptions.Add(m.Name, $"<collection[{count}]>");
                    }
                }
                else
                {
                    if (memberValue == null)
                    {
                        memberDescriptions.Add(m.Name, $"null");
                    }
                    else
                    {
                        memberDescriptions.Add(m.Name, $"{memberValue}");
                    }
                }
            }

            return DictToString(memberDescriptions);
            //return string.Join("\n", memberDescriptions);
        }

        private static string DictToString(Dictionary<string, string> dict)
        {
            if (dict == null || dict.Keys.Count < 1) return string.Empty;

            // calculate the longest key
            var sb = new StringBuilder();
            var padLength = dict.Keys.Max(k => k?.Length) ?? 0;
            var keys = dict.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                sb.AppendLine($"{keys[i].PadRight(padLength, ' ')}: {dict[keys[i]]}");
                //sb.AppendLine($"{keys[i].PadRight(padLength, ' ')}: {dict[keys[i]]}");
            }

            return sb.ToString();
        }
    }
}
