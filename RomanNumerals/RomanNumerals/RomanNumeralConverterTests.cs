using System;
using NUnit.Framework;

namespace RomanNumerals;

public class RomanNumeralConverterTests
{
    [TestCase("", 0)]
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
    [TestCase("XIX", 19)]
    [TestCase("XX", 20)]
    [TestCase("XXX", 30)]
    [TestCase("MMM", 3000)]
    [TestCase("M", 1000)]
    [TestCase("C", 100)]
    [TestCase("CCC", 300)]
    [TestCase("XC", 90)]
    [TestCase("MMCMXLVII", 2947)]
    public void FromRomanNumeralConvertsCorrectly(string input, int expected)
    {
        var actual = RomanNumeralConverter.FromRomanNumeral(input);
        
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase("test")]
    [TestCase("iii")]
    [TestCase("IX  \n \r ")]
    [TestCase(" \t M")]
    [TestCase("XLVIbanana")]
    [TestCase("parachuteMM")]
    [TestCase("  \n \t \r ")]
    public void FromRomanNumeralThrowsFormatExceptionOnInvalidInput(string invalid)
    {
        var ex = Assert.Throws<FormatException>(() => RomanNumeralConverter.FromRomanNumeral(invalid));
        Assert.That(ex, Has.Message.Contains(invalid));
    }

    [Test]
    public void FromRomanNumeralThrowsArgumentNullIfInputIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => RomanNumeralConverter.FromRomanNumeral(null));
    }
}