using Domain.Aggregates.Organization.Errors;
using Domain.Aggregates.Organization.ValueObjects;
using SharedTests.Assertions;

namespace Tests.Domain.ValueObjects.Organization;

[TestClass]
public class OrganizationNameTests
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void Create_ShouldReturnFailure_WhenValueIsMissing(string? value)
    {
        var result = OrganizationName.Create(value);

        result.ShouldFailWith(OrganizationErrors.OrganizationNameRequired());
    }

    [TestMethod]
    [DataRow("A", "A")]
    [DataRow("Empresa", "Empresa")]
    [DataRow("   Empresa   ", "Empresa")]
    public void Create_ShouldReturnSuccess_WhenValueIsValid(string value, string expected)
    {
        var result = OrganizationName.Create(value);

        result.ShouldSucceed();
        Assert.AreEqual(expected, result.Value.Value);
    }

    [TestMethod]
    public void Create_ShouldReturnFailure_WhenValueIsTooLong()
    {
        var value = new string('a', 151);

        var result = OrganizationName.Create(value);

        result.ShouldFailWith(OrganizationErrors.OrganizationNameTooLong(150));
    }

    [TestMethod]
    public void Create_ShouldNormalizeWhitespace()
    {
        var result = OrganizationName.Create("   Minha    Empresa   Ltda   ");

        result.ShouldSucceed();
        Assert.AreEqual("Minha Empresa Ltda", result.Value.Value);
    }

    [TestMethod]
    public void ToString_ShouldReturnValue()
    {
        var result = OrganizationName.Create("Minha Empresa");

        result.ShouldSucceed();
        Assert.AreEqual("Minha Empresa", result.Value.ToString());
    }
}