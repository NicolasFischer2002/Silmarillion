using Domain.Aggregates.Organization.Errors;
using Domain.Aggregates.Organization.ValueObjects;
using SharedTests.Assertions;

namespace Tests.Domain.ValueObjects;

[TestClass]
public class CnpjTests
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void Create_ShouldReturnFailure_WhenValueIsMissing(string? value)
    {
        var result = Cnpj.Create(value);

        result.ShouldFailWith(OrganizationErrors.CnpjRequired());
    }

    [TestMethod]
    [DataRow("123")]
    [DataRow("1234567890123")]
    [DataRow("123456789012345")]
    public void Create_ShouldReturnFailure_WhenLengthIsInvalid(string value)
    {
        var result = Cnpj.Create(value);

        result.ShouldFailWith(OrganizationErrors.CnpjInvalid());
    }

    [TestMethod]
    [DataRow("00000000000000")]
    [DataRow("11111111111111")]
    public void Create_ShouldReturnFailure_WhenAllDigitsAreEqual(string value)
    {
        var result = Cnpj.Create(value);

        result.ShouldFailWith(OrganizationErrors.CnpjInvalid());
    }

    [TestMethod]
    [DataRow("11222333000189")]
    [DataRow("11222333000100")]
    public void Create_ShouldReturnFailure_WhenCheckDigitsAreInvalid(string value)
    {
        var result = Cnpj.Create(value);

        result.ShouldFailWith(OrganizationErrors.CnpjInvalid());
    }

    [TestMethod]
    [DataRow("11222333000181")]
    [DataRow("11.222.333/0001-81")]
    [DataRow(" 11.222.333/0001-81 ")]
    public void Create_ShouldReturnSuccess_WhenValueIsValid(string value)
    {
        var result = Cnpj.Create(value);

        result.ShouldSucceed();
        Assert.AreEqual("11222333000181", result.Value.Value);
    }

    [TestMethod]
    public void GetFormattedValue_ShouldReturnMaskedValue()
    {
        var result = Cnpj.Create("11222333000181");

        result.ShouldSucceed();

        var formatted = result.Value.GetFormattedValue();

        Assert.AreEqual("11.222.333/0001-81", formatted);
    }

    [TestMethod]
    public void ToString_ShouldReturnRawValue()
    {
        var result = Cnpj.Create("11222333000181");

        result.ShouldSucceed();

        Assert.AreEqual("11222333000181", result.Value.ToString());
    }
}