using Domain.Errors;
using Domain.ValueObjects;
using SharedTests.Assertions;

namespace Tests.Domain.ValueObjects;

[TestClass]
public class EmailAddressTests
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void Create_ShouldReturnFailure_WhenValueIsMissing(string? value)
    {
        var result = EmailAddress.Create(value);

        result.ShouldFailWith(IdentityErrors.EmailRequired());
    }

    [TestMethod]
    [DataRow("invalid")]
    [DataRow("invalid@")]
    [DataRow("@domain.com")]
    [DataRow("userdomain.com")]
    [DataRow("user@domain")]
    public void Create_ShouldReturnFailure_WhenValueHasInvalidFormat(string value)
    {
        var result = EmailAddress.Create(value);

        result.ShouldFailWith(IdentityErrors.EmailInvalidFormat());
    }

    [TestMethod]
    [DataRow("user@domain.com")]
    [DataRow("USER@DOMAIN.COM")]
    [DataRow("   user@domain.com   ")]
    public void Create_ShouldReturnSuccess_WhenValueIsValid(string value)
    {
        var result = EmailAddress.Create(value);

        result.ShouldSucceed();
        Assert.AreEqual("user@domain.com", result.Value.Value);
    }

    [TestMethod]
    public void ToString_ShouldReturnValue()
    {
        var result = EmailAddress.Create("user@domain.com");

        result.ShouldSucceed();
        Assert.AreEqual("user@domain.com", result.Value.ToString());
    }
}