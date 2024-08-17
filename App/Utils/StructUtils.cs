using System.Reflection;

namespace App.Utils;

/// <summary>
/// Utility class for working with structs.
/// </summary>
public static class StructUtils
{
    /// <summary>
    /// Changes the value of a read-only property in a struct instance.
    /// </summary>
    /// <typeparam name="T">The struct type.</typeparam>
    /// <param name="structInstance">The struct instance.</param>
    /// <param name="propertyName">The name of the property to change.</param>
    /// <param name="newValue">The new value for the property.</param>
    public static void ChangeReadonlyProperty<T>(ref T structInstance, string propertyName, object newValue)
        where T : struct
    {
        // Get the type of the struct
        var type = typeof(T);

        // Get the field corresponding to the property
        var fieldInfo = type.GetField($"<{propertyName}>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);

        if (fieldInfo != null)
        {
            // Box the struct to modify it
            object boxedStruct = structInstance;
                
            // Set the new value for the field
            fieldInfo.SetValue(boxedStruct, newValue);
                
            // Unbox the modified struct back to the original
            structInstance = (T)boxedStruct;
        }
        else
        {
            Console.WriteLine($"Field for property '{propertyName}' not found.");
        }
    }
}