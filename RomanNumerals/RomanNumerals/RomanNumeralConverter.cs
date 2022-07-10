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
        {
            amount += 4;
            roman = roman.Substring(0, roman.Length - 2);
        }
        else if (roman.EndsWith("V"))
        {
            amount += 5;
            roman = roman.Substring(0, roman.Length - 1);
        }

        if (roman.EndsWith("IX"))
            amount += 9;
        else if (roman.EndsWith("X")) 
            amount += 10;        

        return amount;
    }
}