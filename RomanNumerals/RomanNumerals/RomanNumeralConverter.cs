using System.Linq;

namespace RomanNumerals;

public class RomanNumeralConverter
{
    public static int FromRomanNumeral(string roman)
    {
        var amount = 0;

        amount += roman.Reverse().TakeWhile(c => c.Equals('I')).Count();

        roman = roman.TrimEnd('I');

        if (roman.EndsWith("IV"))
            amount += 4;
        else if (roman.EndsWith("V")) 
            amount += 5;

        return amount;
    }
}