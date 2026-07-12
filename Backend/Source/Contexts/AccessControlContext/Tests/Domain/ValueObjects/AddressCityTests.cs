using Domain.Errors;
using Domain.ValueObjects;
using SharedTests.Assertions;

namespace Tests.Domain.ValueObjects;

[TestClass]
public class AddressCityTests
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void Create_ShouldReturnFailure_WhenValueIsMissing(string? value)
    {
        var result = AddressCity.Create(value);

        result.ShouldFailWith(OrganizationErrors.CityRequired());
    }

    [TestMethod]
    [DataRow("A")]
    [DataRow("RJ")]
    public void Create_ShouldReturnFailure_WhenValueIsTooShort(string value)
    {
        var result = AddressCity.Create(value);

        result.ShouldFailWith(OrganizationErrors.CityTooShort(3));
    }

    [TestMethod]
    public void Create_ShouldReturnFailure_WhenValueIsTooLong()
    {
        var value = new string('a', 101);

        var result = AddressCity.Create(value);

        result.ShouldFailWith(OrganizationErrors.CityTooLong(100));
    }

    [TestMethod]
    [DataRow("São Paulo")]
    [DataRow("   São Paulo   ")]
    [DataRow("São    Paulo")]
    public void Create_ShouldReturnSuccess_WhenValueIsValid(string value)
    {
        var result = AddressCity.Create(value);

        result.ShouldSucceed();
        Assert.AreEqual("São Paulo", result.Value.Value);
    }

    [TestMethod]
    public void Create_ShouldNormalizeWhitespace()
    {
        var result = AddressCity.Create("   Belo    Horizonte   ");

        result.ShouldSucceed();
        Assert.AreEqual("Belo Horizonte", result.Value.Value);
    }

    [TestMethod]
    public void ToString_ShouldReturnValue()
    {
        var result = AddressCity.Create("Curitiba");

        result.ShouldSucceed();
        Assert.AreEqual("Curitiba", result.Value.ToString());
    }
}