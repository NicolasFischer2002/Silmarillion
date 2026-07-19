using Domain.Aggregates.Organization.Errors;
using Domain.Aggregates.Organization.ValueObjects;
using SharedTests.Assertions;

namespace Tests.Domain.ValueObjects.Organization;

[TestClass]
public class AddressComplementTests
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void Create_ShouldReturnSuccess_WhenValueIsNullOrEmpty(string? value)
    {
        var result = AddressComplement.Create(value);

        result.ShouldSucceed();
        Assert.AreEqual(string.Empty, result.Value.Value);
    }

    [TestMethod]
    public void Create_ShouldReturnFailure_WhenValueIsTooLong()
    {
        var value = new string('a', 101);

        var result = AddressComplement.Create(value);

        result.ShouldFailWith(OrganizationErrors.ComplementTooLong(100));
    }

    [TestMethod]
    [DataRow("Apto 101")]
    [DataRow("   Apto 101   ")]
    [DataRow("Apto    101")]
    public void Create_ShouldReturnSuccess_WhenValueIsValid(string value)
    {
        var result = AddressComplement.Create(value);

        result.ShouldSucceed();
        Assert.AreEqual("Apto 101", result.Value.Value);
    }

    [TestMethod]
    public void Create_ShouldNormalizeWhitespace()
    {
        var result = AddressComplement.Create("   Bloco    B   ");

        result.ShouldSucceed();
        Assert.AreEqual("Bloco B", result.Value.Value);
    }

    [TestMethod]
    public void ToString_ShouldReturnValue()
    {
        var result = AddressComplement.Create("Apto 202");

        result.ShouldSucceed();
        Assert.AreEqual("Apto 202", result.Value.ToString());
    }
}