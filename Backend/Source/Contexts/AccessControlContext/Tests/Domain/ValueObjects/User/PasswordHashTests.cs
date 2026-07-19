using Domain.Aggregates.User.Errors;
using Domain.Aggregates.User.ValueObjects;
using SharedTests.Assertions;

namespace Tests.Domain.ValueObjects.User;

[TestClass]
public class PasswordHashTests
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void Create_ShouldReturnFailure_WhenValueIsMissing(string? value)
    {
        var result = PasswordHash.Create(value);

        result.ShouldFailWith(UserErrors.PasswordHashRequired());
    }

    [TestMethod]
    [DataRow("hash")]
    [DataRow("   hash")]
    [DataRow("hash   ")]
    [DataRow("   hash   ")]
    [DataRow("$2a$12$abcdefghijklmnopqrstuvABCDEFGHIJKLMN0123456789")]
    public void Create_ShouldReturnSuccess_WhenValueIsValid(string value)
    {
        var result = PasswordHash.Create(value);

        result.ShouldSucceed();
        Assert.AreEqual(value.Trim(), result.Value.Value);
    }

    [TestMethod]
    public void Create_ShouldTrimWhitespace()
    {
        var result = PasswordHash.Create("   my-password-hash   ");

        result.ShouldSucceed();
        Assert.AreEqual("my-password-hash", result.Value.Value);
    }

    [TestMethod]
    public void ToString_ShouldNotExposeHash()
    {
        var result = PasswordHash.Create("my-secret-hash");

        result.ShouldSucceed();
        Assert.AreEqual("[PASSWORD_HASH]", result.Value.ToString());
    }
}