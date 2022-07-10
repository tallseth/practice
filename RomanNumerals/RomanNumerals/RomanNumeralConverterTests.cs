using NUnit.Framework;

namespace RomanNumerals;

public class RomanNumeralConverterTests
{
    [TestCase("I", 1)]
    public void FromRomanNumeralConvertsCorrectly(string input, int expected)
    {
        var actual = RomanNumeralConverter.FromRomanNumeral(input);
        
        Assert.That(actual, Is.EqualTo(expected));
    }
}