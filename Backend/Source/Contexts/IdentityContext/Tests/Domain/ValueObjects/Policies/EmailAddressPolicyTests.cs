using Domain.Errors;
using Domain.Policies.Domain.Policies;
using SharedTests.Assertions;

namespace Tests.Domain.ValueObjects.Policies;

[TestClass]
public class EmailAddressPolicyTests
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void Validate_ShouldReturnFailure_WhenValueIsMissing(string? value)
    {
        var result = EmailAddressPolicy.Validate(value);

        result.ShouldFailWith(IdentityErrors.EmailRequired());
    }

    [TestMethod]
    [DataRow("invalid")]
    [DataRow("invalid@")]
    [DataRow("@domain.com")]
    [DataRow("userdomain.com")]
    [DataRow("user@domain")]
    public void Validate_ShouldReturnFailure_WhenValueHasInvalidFormat(string value)
    {
        var result = EmailAddressPolicy.Validate(value);

        result.ShouldFailWith(IdentityErrors.EmailInvalidFormat());
    }

    [TestMethod]
    [DataRow("USER@DOMAIN.COM")]
    [DataRow("   User@Domain.Com   ")]
    public void Validate_ShouldReturnSuccessAndNormalize_WhenValueIsValid(string value)
    {
        var result = EmailAddressPolicy.Validate(value);

        result.ShouldSucceed();
        Assert.AreEqual("user@domain.com", result.Value);
    }

    [TestMethod]
    public void Validate_ShouldReturnFailure_WhenValueIsTooLong()
    {
        var input = new string('a', EmailAddressPolicy.MaxLength + 1);
        var result = EmailAddressPolicy.Validate(input);

        result.ShouldFailWith(IdentityErrors.EmailTooLong(EmailAddressPolicy.MaxLength));
    }
}