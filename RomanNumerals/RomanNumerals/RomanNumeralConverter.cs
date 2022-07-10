using System;

namespace RomanNumerals;

public class RomanNumeralConverter
{
    public static int FromRomanNumeral(string roman)
    {
        var amount = 0;

        foreach (var definition in new [] {I, IV, V, IX, X, XL, L, XC, C, CM, M})
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

    private static readonly NumeralDefinition I = new() { Glyph = "I", Value = 1 };
    private static readonly NumeralDefinition IV = new() { Glyph = "IV", Value = 4 };
    private static readonly NumeralDefinition V = new() { Glyph = "V", Value = 5 };
    private static readonly NumeralDefinition IX = new() { Glyph = "IX", Value = 9 };
    private static readonly NumeralDefinition X = new() { Glyph = "X", Value = 10 };
    private static readonly NumeralDefinition XL = new() { Glyph = "XL", Value = 40 };
    private static readonly NumeralDefinition L = new() { Glyph = "L", Value = 50 };
    private static readonly NumeralDefinition XC = new() { Glyph = "XC", Value = 90 };
    private static readonly NumeralDefinition C = new() { Glyph = "C", Value = 100 };
    private static readonly NumeralDefinition CM = new() { Glyph = "CM", Value = 900 };
    private static readonly NumeralDefinition M = new() { Glyph = "M", Value = 1000 };

    private class NumeralDefinition
    {
        public string Glyph;
        public int Value;
        public int Repetitions => ValueIsPowerOfTen() ? 3 : 1;
        public int Length => Glyph.Length;
        
        private bool ValueIsPowerOfTen()
        {
            return Math.Abs(Math.Log10(Value) - Math.Floor(Math.Log10(Value))) < 0.01;
        }
    }
}