using System.Reflection;

namespace Goodtocode.DotNet.Extensions;

/// <summary>
/// object Extensions
/// </summary>
public static class ObjectExtension
{
    /// <summary>
    /// Get list of properties decorated with the passed attribute
    /// </summary>
    /// <param name="item"></param>
    /// <param name="myAttribute"></param>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetPropertiesByAttribute(this object item, Type myAttribute)
    {
        var returnValue = item.GetType().GetTypeInfo().DeclaredProperties.Where(
            p => p.GetCustomAttributes(myAttribute, false).Any());

        return returnValue;
    }

    /// <summary>
    /// Safe Type Casting based on .NET default() method
    /// </summary>
    /// <typeparam name="TDestination">default(DestinationType)</typeparam>
    /// <param name="item">Item to default.</param>
    /// <returns>default(DestinationType)</returns>
    public static TDestination? DefaultSafe<TDestination>(this object item)
    {
        var returnValue = TypeExtension.InvokeConstructorOrDefault<TDestination>();

        try
        {
            if (item != null)
            {
                returnValue = (TDestination)item;
            }
        }
        catch
        {
            returnValue = TypeExtension.InvokeConstructorOrDefault<TDestination>();
        }

        return returnValue;
    }

    /// <summary>
    /// Safe type casting via (TDestination)item method.
    /// If cast fails, will return constructed object
    /// </summary>
    /// <typeparam name="TDestination">Type to default, or create new()</typeparam>
    /// <param name="item">Item to cast</param>
    /// <returns>Cast result via (TDestination)item, or item.Fill(), or new TDestination().</returns>
    public static TDestination CastSafe<TDestination>(this object item) where TDestination : new()
    {
        var returnValue = new TDestination();

        try
        {
            returnValue = item != null ? (TDestination)item : returnValue;
        }
        catch (InvalidCastException)
        {
            returnValue = new TDestination();
        }

        return returnValue;
    }

    /// <summary>
    /// Safe Type Casting based on Default.{Type} conventions.
    /// If cast fails, will attempt the slower Fill() of data via reflection
    /// </summary>
    /// <typeparam name="TDestination">Type to default, or create new()</typeparam>
    /// <param name="item">Item to cast</param>
    /// <returns>Defaulted type, or created new()</returns>
    public static TDestination CastOrCopyProperties<TDestination>(this object item) where TDestination : new()
    {
        var returnValue = new TDestination();

        try
        {
            returnValue = item != null ? (TDestination)item : returnValue;
        }
        catch (InvalidCastException)
        {
            returnValue.CopyPropertiesSafe(item);
        }

        return returnValue;
    }

    /// <summary>
    /// Safe Type Casting based on Default.{Type} conventions.
    /// If cast fails, will attempt the slower Fill() of data via reflection
    /// </summary>
    /// <typeparam name="TDestination">Type to default, or create new()</typeparam>
    /// <param name="item">Item to cast</param>
    /// <returns>Defaulted type, or created new()</returns>
    public static TDestination CopyPropertiesSafe<TDestination>(this object item) where TDestination : new()
    {
        var returnValue = new TDestination();
        returnValue.CopyPropertiesSafe(item);
        return returnValue;
    }

    /// <summary>
    /// Item to exception-safe cast to string
    /// </summary>
    /// <param name="item">Item to cast</param>
    /// <returns>Converted string, or ""</returns>
    public static string? ToStringSafe(this object item)
    {
        var returnValue = string.Empty;

        if (item == null == false)
        {
            returnValue = item.ToString();
        }

        return returnValue;
    }


    /// <summary>
    /// Fills this object with another object's data, by matching property names
    /// </summary>
    /// <typeparam name="T">Type of original object.</typeparam>
    /// <param name="item">Destination object to fill</param>
    /// <param name="sourceItem">Source object</param>
    public static void CopyPropertiesSafe<T>(this T item, object sourceItem)
    {
        var sourceType = sourceItem.GetType();

        foreach (PropertyInfo sourceProperty in sourceType.GetRuntimeProperties())
        {
            PropertyInfo? destinationProperty = typeof(T).GetRuntimeProperty(sourceProperty.Name);
            if (destinationProperty != null && destinationProperty.CanWrite)
            {
                // Copy data only for Primitive-ish types including Value types, Guid, String, etc.
                Type destinationPropertyType = destinationProperty.PropertyType;
                if (destinationPropertyType.GetTypeInfo().IsPrimitive || destinationPropertyType.GetTypeInfo().IsValueType
                    || (destinationPropertyType == typeof(string)) || (destinationPropertyType == typeof(Guid)))
                {
                    destinationProperty.SetValue(item, sourceProperty.GetValue(sourceItem, null), null);
                }
            }
        }
    }
}