using System.Linq;

namespace RomanNumerals;

public class RomanNumeralConverter
{
    public static int FromRomanNumeral(string roman)
    {
        var amount = 0;

        (amount, roman) = CountTermIfPresent(amount, roman, "III", 3);
        (amount, roman) = CountTermIfPresent(amount, roman, "II", 2);
        (amount, roman) = CountTermIfPresent(amount, roman, "I", 1);
        (amount, roman) = CountTermIfPresent(amount, roman, "IV", 4);
        (amount, roman) = CountTermIfPresent(amount, roman, "V", 5);
        (amount, roman) = CountTermIfPresent(amount, roman, "IX", 9);
        (amount, roman) = CountTermIfPresent(amount, roman, "X", 10);
        (amount, roman) = CountTermIfPresent(amount, roman, "X", 10);
        (amount, roman) = CountTermIfPresent(amount, roman, "X", 10);

        return amount;
    }

    private static (int, string) CountTermIfPresent(int accumulator, string origin, string term, int termAmount)
    {
        if (origin.EndsWith(term))
        {
            accumulator += termAmount;
            origin = origin.Substring(0, origin.Length - term.Length);
        }
        
        return (accumulator, origin);
    }
}