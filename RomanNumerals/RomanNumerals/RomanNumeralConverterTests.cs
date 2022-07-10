using NUnit.Framework;

namespace RomanNumerals;

public class RomanNumeralConverterTests
{
    [TestCase("I", 1)]
    [TestCase("II", 2)]
    [TestCase("III", 3)]
    [TestCase("IV", 4)]
    [TestCase("V", 5)]
    [TestCase("VI", 6)]
    [TestCase("VII", 7)]
    [TestCase("VIII", 8)]
    [TestCase("IX", 9)]
    [TestCase("X", 10)]
    [TestCase("XI", 11)]
    [TestCase("XII", 12)]
    [TestCase("XIII", 13)]
    [TestCase("XIV", 14)]
    [TestCase("XV", 15)]
    [TestCase("XVI", 16)]
    [TestCase("XVII", 17)]
    [TestCase("XVIII", 18)]
    public void FromRomanNumeralConvertsCorrectly(string input, int expected)
    {
        var actual = RomanNumeralConverter.FromRomanNumeral(input);
        
        Assert.That(actual, Is.EqualTo(expected));
    }
}