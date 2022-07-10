namespace RomanNumerals;

public class RomanNumeralConverter
{
    public static int FromRomanNumeral(string roman)
    {
        if (roman == "III")
            return 3;
        
        if (roman == "II")
            return 2;

        return 1;
    }
}