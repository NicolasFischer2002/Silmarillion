using Domain.Aggregates.Organizations.Errors;
using Domain.Aggregates.Organizations.ValueObjects;
using SharedTests.Assertions;

namespace Tests.Domain.ValueObjects;

[TestClass]
public class AddressZipCodeTests
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void Create_ShouldReturnFailure_WhenValueIsMissing(string? value)
    {
        var result = AddressZipCode.Create(value);

        result.ShouldFailWith(OrganizationErrors.ZipCodeRequired());
    }

    [TestMethod]
    [DataRow("123")]
    [DataRow("1234567")]
    [DataRow("123456789")]
    public void Create_ShouldReturnFailure_WhenLengthIsInvalid(string value)
    {
        var result = AddressZipCode.Create(value);

        result.ShouldFailWith(OrganizationErrors.ZipCodeInvalid());
    }

    [TestMethod]
    [DataRow("00000000")]
    [DataRow("11111111")]
    public void Create_ShouldReturnFailure_WhenAllDigitsAreEqual(string value)
    {
        var result = AddressZipCode.Create(value);

        result.ShouldFailWith(OrganizationErrors.ZipCodeInvalid());
    }

    [TestMethod]
    [DataRow("12345678")]
    [DataRow("12345-678")]
    [DataRow(" 12345-678 ")]
    public void Create_ShouldReturnSuccess_WhenValueIsValid(string value)
    {
        var result = AddressZipCode.Create(value);

        result.ShouldSucceed();
        Assert.AreEqual("12345678", result.Value.Value);
    }

    [TestMethod]
    public void Create_ShouldNormalizeValue_RemovingNonDigits()
    {
        var result = AddressZipCode.Create("12.345-678");

        result.ShouldSucceed();
        Assert.AreEqual("12345678", result.Value.Value);
    }

    [TestMethod]
    public void GetFormattedValue_ShouldReturnMaskedValue()
    {
        var result = AddressZipCode.Create("12345678");

        result.ShouldSucceed();

        var formatted = result.Value.GetFormattedValue();

        Assert.AreEqual("12345-678", formatted);
    }

    [TestMethod]
    public void ToString_ShouldReturnRawValue()
    {
        var result = AddressZipCode.Create("12345678");

        result.ShouldSucceed();

        Assert.AreEqual("12345678", result.Value.ToString());
    }
}