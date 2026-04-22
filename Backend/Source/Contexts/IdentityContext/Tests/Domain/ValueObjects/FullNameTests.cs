using Domain.Errors;
using Domain.ValueObjects;
using SharedTests.Assertions;

namespace Tests.Domain.ValueObjects;

[TestClass]
public class FullNameTests
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void Create_ShouldReturnFailure_WhenValueIsMissing(string? value)
    {
        var result = FullName.Create(value);

        result.ShouldFailWith(IdentityErrors.FullNameRequired());
    }

    [TestMethod]
    [DataRow("Ana")]
    [DataRow("Jo")]
    [DataRow("A B")]
    [DataRow("123456789")]
    public void Create_ShouldReturnFailure_WhenValueIsTooShort(string value)
    {
        var result = FullName.Create(value);

        result.ShouldFailWith(IdentityErrors.FullNameTooShort(10));
    }

    [TestMethod]
    [DataRow("João da Silva")]
    [DataRow("   João da Silva   ")]
    [DataRow("João    da    Silva")]
    public void Create_ShouldReturnSuccess_WhenValueIsValid(string value)
    {
        var result = FullName.Create(value);

        result.ShouldSucceed();
        Assert.AreEqual("João da Silva", result.Value.Value);
    }

    [TestMethod]
    public void Create_ShouldReturnFailure_WhenValueIsTooLong()
    {
        var result = FullName.Create(new string('a', 101));

        result.ShouldFailWith(IdentityErrors.FullNameTooLong(100));
    }

    [TestMethod]
    public void ToString_ShouldReturnValue()
    {
        var fullName = FullName.Create("João da Silva");

        fullName.ShouldSucceed();
        Assert.AreEqual("João da Silva", fullName.Value.ToString());
    }
}