using System.Linq;
using NUnit.Framework;

namespace RomanNumerals;

public class RomanNumeralConverter
{
    public static int FromRomanNumeral(string roman)
    {
        var amount = 0;

        foreach (var definition in new [] {I, IV, V, IX, X})
        {
            (amount, roman) = CountTermIfPresent(amount, roman, definition);
        }

        return amount;
    }

    private static (int, string) CountTermIfPresent(int accumulator, string origin, NumeralDefinition definition)
    {
        for (int i = 0; i < definition.Repetitions; i++)
        {
            if (origin.EndsWith(definition.Glyph))
            {
                accumulator += definition.Value;
                origin = origin.Substring(0, origin.Length - definition.Length);
            }            
        }

        return (accumulator, origin);
    }

    private static readonly NumeralDefinition I = new() { Glyph = "I", Value = 1, Repetitions = 3 };
    private static readonly NumeralDefinition IV = new() { Glyph = "IV", Value = 4, Repetitions = 1 };
    private static readonly NumeralDefinition V = new() { Glyph = "V", Value = 5, Repetitions = 1 };
    private static readonly NumeralDefinition IX = new() { Glyph = "IX", Value = 9, Repetitions = 1 };
    private static readonly NumeralDefinition X = new() { Glyph = "X", Value = 10, Repetitions = 3 };

    private class NumeralDefinition
    {
        public string Glyph;
        public int Value;
        public int Repetitions;
        public int Length => Glyph.Length;
    }
}