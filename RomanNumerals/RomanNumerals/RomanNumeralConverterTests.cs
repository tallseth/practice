using NUnit.Framework;

namespace RomanNumerals;

public class RomanNumeralConverterTests
{
    [TestCase("I", 1)]
    [TestCase("II", 2)]
    [TestCase("III", 3)]
    public void FromRomanNumeralConvertsCorrectly(string input, int expected)
    {
        var actual = RomanNumeralConverter.FromRomanNumeral(input);
        
        Assert.That(actual, Is.EqualTo(expected));
    }
}