using Domain.Errors;
using Domain.ValueObjects;
using SharedTests.Assertions;

namespace Tests.Domain.ValueObjects;

[TestClass]
public class AddressNumberTests
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void Create_ShouldReturnFailure_WhenValueIsMissing(string? value)
    {
        var result = AddressNumber.Create(value);

        result.ShouldFailWith(OrganizationErrors.NumberRequired());
    }

    [TestMethod]
    public void Create_ShouldReturnFailure_WhenValueIsTooLong()
    {
        var value = new string('1', 11);

        var result = AddressNumber.Create(value);

        result.ShouldFailWith(OrganizationErrors.NumberTooLong(10));
    }

    [TestMethod]
    [DataRow("1", "1")]
    [DataRow("123", "123")]
    [DataRow("10A", "10A")]
    [DataRow("   123   ", "123")]
    [DataRow("12    B", "12 B")]
    public void Create_ShouldReturnSuccess_WhenValueIsValid(
    string value,
    string expected)
    {
        var result = AddressNumber.Create(value);

        result.ShouldSucceed();
        Assert.AreEqual(expected, result.Value.Value);
    }

    [TestMethod]
    public void Create_ShouldNormalizeWhitespace()
    {
        var result = AddressNumber.Create("   123    A   ");

        result.ShouldSucceed();
        Assert.AreEqual("123 A", result.Value.Value);
    }

    [TestMethod]
    public void ToString_ShouldReturnValue()
    {
        var result = AddressNumber.Create("123");

        result.ShouldSucceed();
        Assert.AreEqual("123", result.Value.ToString());
    }
}