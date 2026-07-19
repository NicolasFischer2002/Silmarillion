using Domain.Aggregates.Session.Errors;
using Domain.Aggregates.Session.ValueObjects;
using SharedTests.Assertions;

namespace Tests.Domain.ValueObjects.Session;

[TestClass]
public class RefreshTokenHashTests
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void Create_ShouldReturnFailure_WhenValueIsMissing(string? value)
    {
        var result = RefreshTokenHash.Create(value);

        result.ShouldFailWith(SessionErrors.RefreshTokenHashRequired());
    }

    [TestMethod]
    public void Create_ShouldReturnFailure_WhenValueIsTooLong()
    {
        var value = new string('a', 513);

        var result = RefreshTokenHash.Create(value);

        result.ShouldFailWith(
            RefreshTokenHashErrors.RefreshTokenHashTooLong(512));
    }

    [TestMethod]
    [DataRow("8A0D6E9E7B8D4D9F2A6B3F5E9C1D8A7B")]
    [DataRow("   8A0D6E9E7B8D4D9F2A6B3F5E9C1D8A7B   ")]
    public void Create_ShouldReturnSuccess_WhenValueIsValid(string value)
    {
        var result = RefreshTokenHash.Create(value);

        result.ShouldSucceed();

        Assert.AreEqual(
            "8A0D6E9E7B8D4D9F2A6B3F5E9C1D8A7B",
            result.Value.Value);
    }

    [TestMethod]
    public void Create_ShouldNormalizeWhitespace()
    {
        var result = RefreshTokenHash.Create(
            "   ABCDEF1234567890ABCDEF1234567890   ");

        result.ShouldSucceed();

        Assert.AreEqual(
            "ABCDEF1234567890ABCDEF1234567890",
            result.Value.Value);
    }

    [TestMethod]
    public void ToString_ShouldReturnValue()
    {
        var result = RefreshTokenHash.Create(
            "ABCDEF1234567890ABCDEF1234567890");

        result.ShouldSucceed();

        Assert.AreEqual(
            "ABCDEF1234567890ABCDEF1234567890",
            result.Value.ToString());
    }
}