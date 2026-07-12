using Domain.Aggregates.Organizations.Errors;
using Domain.Aggregates.Organizations.ValueObjects;
using SharedTests.Assertions;

namespace Tests.Domain.ValueObjects;

[TestClass]
public class AddressStreetTests
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void Create_ShouldReturnFailure_WhenValueIsMissing(string? value)
    {
        var result = AddressStreet.Create(value);

        result.ShouldFailWith(OrganizationErrors.StreetRequired());
    }

    [TestMethod]
    public void Create_ShouldReturnFailure_WhenValueIsTooLong()
    {
        var value = new string('a', 101);

        var result = AddressStreet.Create(value);

        result.ShouldFailWith(OrganizationErrors.StreetTooLong(100));
    }

    [TestMethod]
    [DataRow("Rua A", "Rua A")]
    [DataRow("   Rua das Flores   ", "Rua das Flores")]
    [DataRow("Rua    das    Flores", "Rua das Flores")]
    public void Create_ShouldReturnSuccess_WhenValueIsValid(string value, string expected)
    {
        var result = AddressStreet.Create(value);

        result.ShouldSucceed();
        Assert.AreEqual(expected, result.Value.Value);
    }

    [TestMethod]
    public void Create_ShouldNormalizeWhitespace()
    {
        var result = AddressStreet.Create("   Av.    Brasil   ");

        result.ShouldSucceed();
        Assert.AreEqual("Av. Brasil", result.Value.Value);
    }

    [TestMethod]
    public void ToString_ShouldReturnValue()
    {
        var result = AddressStreet.Create("Rua ABC");

        result.ShouldSucceed();
        Assert.AreEqual("Rua ABC", result.Value.ToString());
    }
}