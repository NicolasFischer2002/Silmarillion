using SharedKernel.Errors;
using SharedKernel.Policies;
using SharedTests.Assertions;

namespace SharedTests.Policies;

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

        result.ShouldFailWith(
            EmailAddressPolicyErrors.EmailRequired()
        );
    }

    [TestMethod]
    [DataRow("invalid")]
    [DataRow("invalid@")]
    [DataRow("@domain.com")]
    [DataRow("userdomain.com")]
    [DataRow("user@domain")]
    [DataRow("user@@domain.com")]
    public void Validate_ShouldReturnFailure_WhenFormatIsInvalid(string value)
    {
        var result = EmailAddressPolicy.Validate(value);

        result.ShouldFailWith(
            EmailAddressPolicyErrors.EmailInvalidFormat()
        );
    }

    [TestMethod]
    public void Validate_ShouldReturnFailure_WhenValueIsTooLong()
    {
        var localPart = new string('a', 249);
        var email = $"{localPart}@a.com";

        var result = EmailAddressPolicy.Validate(email);

        result.ShouldFailWith(
            EmailAddressPolicyErrors.EmailTooLong(
                EmailAddressPolicy.MaxLength
            )
        );
    }

    [TestMethod]
    [DataRow("user@domain.com", "user@domain.com")]
    [DataRow("USER@DOMAIN.COM", "user@domain.com")]
    [DataRow("   User@Domain.Com   ", "user@domain.com")]
    public void Validate_ShouldReturnSuccess_WhenValueIsValid(
        string value,
        string expected)
    {
        var result = EmailAddressPolicy.Validate(value);

        result.ShouldSucceed();

        Assert.AreEqual(expected, result.Value);
    }

    [TestMethod]
    public void Validate_ShouldNormalizeToLowerCase()
    {
        var result = EmailAddressPolicy.Validate(
            "USER@DOMAIN.COM"
        );

        result.ShouldSucceed();

        Assert.AreEqual(
            "user@domain.com",
            result.Value
        );
    }

    [TestMethod]
    public void Validate_ShouldNormalizeWhitespace()
    {
        var result = EmailAddressPolicy.Validate(
            "   user@domain.com   "
        );

        result.ShouldSucceed();

        Assert.AreEqual(
            "user@domain.com",
            result.Value
        );
    }
}