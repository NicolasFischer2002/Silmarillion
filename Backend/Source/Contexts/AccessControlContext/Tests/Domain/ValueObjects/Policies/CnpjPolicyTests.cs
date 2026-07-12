using Domain.Aggregates.Organization.Errors;
using Domain.Aggregates.Organization.Policies;
using SharedTests.Assertions;

namespace Tests.Domain.ValueObjects.Policies;

[TestClass]
public class CnpjPolicyTests
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void Validate_ShouldReturnFailure_WhenValueIsMissing(string? value)
    {
        var result = CnpjPolicy.Validate(value);

        result.ShouldFailWith(OrganizationErrors.CnpjRequired());
    }

    [TestMethod]
    [DataRow("1234567890123")]     // 13 digits
    [DataRow("123456789012345")]   // 15 digits
    [DataRow("123")]               // too short
    public void Validate_ShouldReturnFailure_WhenLengthIsInvalid(string value)
    {
        var result = CnpjPolicy.Validate(value);

        result.ShouldFailWith(OrganizationErrors.CnpjInvalid());
    }

    [TestMethod]
    [DataRow("00000000000000")]
    [DataRow("11111111111111")]
    [DataRow("99999999999999")]
    public void Validate_ShouldReturnFailure_WhenAllDigitsAreEqual(string value)
    {
        var result = CnpjPolicy.Validate(value);

        result.ShouldFailWith(OrganizationErrors.CnpjInvalid());
    }

    [TestMethod]
    [DataRow("11222333000181")]
    [DataRow("11.222.333/0001-81")]
    [DataRow(" 11.222.333/0001-81 ")]
    public void Validate_ShouldReturnSuccess_WhenValueIsValid(string value)
    {
        var result = CnpjPolicy.Validate(value);

        result.ShouldSucceed();
        Assert.AreEqual("11222333000181", result.Value);
    }

    [TestMethod]
    [DataRow("11222333000189")]
    [DataRow("11222333000100")]
    public void Validate_ShouldReturnFailure_WhenCheckDigitsAreInvalid(string value)
    {
        var result = CnpjPolicy.Validate(value);

        result.ShouldFailWith(OrganizationErrors.CnpjInvalid());
    }

    [TestMethod]
    public void Validate_ShouldNormalizeValue_RemovingNonDigits()
    {
        var result = CnpjPolicy.Validate("11.222.333/0001-81");

        result.ShouldSucceed();
        Assert.AreEqual("11222333000181", result.Value);
    }
}