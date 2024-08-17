namespace App.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Converts a string to an acronym by taking the first letter and removing vowels from the rest of the string.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <returns>The acronym of the input string.</returns>
    public static string ToAcronym(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        // Convert to uppercase
        input = input.ToUpper();

        // Take the first letter
        var firstLetter = input[0];

        // Remove vowels from the rest of the string
        var rest = new string(input.Skip(1).Where(c => !"AEIOUY".Contains(c)).ToArray());

        // Combine the first letter and the rest without vowels
        return firstLetter + rest;
    }
}