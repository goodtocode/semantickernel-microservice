using System.Reflection;

namespace Goodtocode.DotNet.Extensions;

/// <summary>
/// Extends System.Type
/// </summary>
public static class TypeExtension
{
    /// <summary>
    /// Invokes the parameterless constructor. If no parameterless constructor, returns default()
    /// </summary>
    /// <typeparam name="T">Type to invoke</typeparam>
    public static T? InvokeConstructorOrDefault<T>()
    {
        var returnValue = default(T);

        if (TypeExtension.HasParameterlessConstructor<T>())
        {
            returnValue = Activator.CreateInstance<T>();
        }

        return returnValue;
    }

    /// <summary>
    /// Determines if type has a parameterless constructor
    /// </summary>
    /// <typeparam name="T">Type to interrogate for parameterless constructor</typeparam>
    /// <returns></returns>
    public static bool HasParameterlessConstructor<T>()
    {
        IEnumerable<ConstructorInfo> constructors = typeof(T).GetTypeInfo().DeclaredConstructors;
        return constructors.Where(x => x.GetParameters().Count() == 0).Any();
    }

    /// <summary>
    /// Determines if type has a parameterless constructor
    /// </summary>
    /// <param name="item">Type to interrogate for parameterless constructor</param>
    /// <returns></returns>
    public static bool HasParameterlessConstructor(this Type item)
    {
        IEnumerable<ConstructorInfo> constructors = item.GetTypeInfo().DeclaredConstructors;
        return constructors.Where(x => x.GetParameters().Count() == 0).Any();
    }
}