using Domain.Aggregates.Session.Errors;
using Domain.Aggregates.Session.ValueObjects;
using SharedKernel.Text;
using SharedTests.Assertions;

namespace Tests.Domain.ValueObjects.Session;

[TestClass]
public class UserAgentTests
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void Create_ShouldReturnFailure_WhenValueIsMissing(string? value)
    {
        var result = UserAgent.Create(value);

        result.ShouldFailWith(UserAgentErrors.UserAgentRequired());
    }

    [TestMethod]
    public void Create_ShouldReturnFailure_WhenValueIsTooLong()
    {
        var value = new string('a', 513);

        var result = UserAgent.Create(value);

        result.ShouldFailWith(
            UserAgentErrors.UserAgentTooLong(512));
    }

    [TestMethod]
    [DataRow("Mozilla/5.0")]
    [DataRow("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 Chrome/138.0.0.0 Safari/537.36")]
    [DataRow("   Mozilla/5.0   ")]
    public void Create_ShouldReturnSuccess_WhenValueIsValid(string value)
    {
        var result = UserAgent.Create(value);

        result.ShouldSucceed();

        Assert.AreEqual(
            StringNormalizer.Normalize(value),
            result.Value.Value);
    }

    [TestMethod]
    public void Create_ShouldNormalizeWhitespace()
    {
        var result = UserAgent.Create(
            "   Mozilla/5.0    Chrome/138.0.0.0   ");

        result.ShouldSucceed();

        Assert.AreEqual(
            "Mozilla/5.0 Chrome/138.0.0.0",
            result.Value.Value);
    }

    [TestMethod]
    public void ToString_ShouldReturnValue()
    {
        var result = UserAgent.Create(
            "Mozilla/5.0");

        result.ShouldSucceed();

        Assert.AreEqual(
            "Mozilla/5.0",
            result.Value.ToString());
    }
}