using Domain.Aggregates.Organization.Errors;
using Domain.Aggregates.Organization.Policies;
using SharedTests.Assertions;

namespace Tests.Domain.ValueObjects.Policies;

[TestClass]
public class AddressZipCodePolicyTests
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void Validate_ShouldReturnFailure_WhenValueIsMissing(string? value)
    {
        var result = AddressZipCodePolicy.Validate(value);

        result.ShouldFailWith(OrganizationErrors.ZipCodeRequired());
    }

    [TestMethod]
    [DataRow("1234567")]
    [DataRow("123456789")]   
    [DataRow("1234")]
    public void Validate_ShouldReturnFailure_WhenLengthIsInvalid(string value)
    {
        var result = AddressZipCodePolicy.Validate(value);

        result.ShouldFailWith(OrganizationErrors.ZipCodeInvalid());
    }

    [TestMethod]
    [DataRow("00000000")]
    [DataRow("11111111")]
    [DataRow("99999999")]
    public void Validate_ShouldReturnFailure_WhenAllDigitsAreEqual(string value)
    {
        var result = AddressZipCodePolicy.Validate(value);

        result.ShouldFailWith(OrganizationErrors.ZipCodeInvalid());
    }

    [TestMethod]
    [DataRow("12345678", "12345678")]
    [DataRow("01310100", "01310100")]
    [DataRow("70040010", "70040010")]
    [DataRow("20040002", "20040002")]
    public void Validate_ShouldReturnSuccess_WhenValueIsValid(string value, string expected)   
	{
        var result = AddressZipCodePolicy.Validate(value);

        result.ShouldSucceed();
        Assert.AreEqual(expected, result.Value);
    }
}