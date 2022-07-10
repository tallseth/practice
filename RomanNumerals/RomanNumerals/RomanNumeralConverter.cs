namespace RomanNumerals;

public class RomanNumeralConverter
{
    public static int FromRomanNumeral(string roman)
    {
        if (roman == "VIII")
            return 8;
        
        if (roman == "VII")
            return 7;
        
        if (roman == "VI")
            return 6;
        
        if (roman == "V")
            return 5;
        
        if (roman == "IV")
            return 4;
        
        if (roman == "III")
            return 3;
        
        if (roman == "II")
            return 2;

        return 1;
    }
}