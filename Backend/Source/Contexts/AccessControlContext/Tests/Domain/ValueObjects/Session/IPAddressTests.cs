using Domain.Aggregates.Session.Errors;
using Domain.Aggregates.Session.ValueObjects;
using SharedTests.Assertions;

namespace Tests.Domain.ValueObjects.Session;

[TestClass]
public class IPAddressTests
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void Create_ShouldReturnFailure_WhenValueIsMissing(string? value)
    {
        var result = IPAddress.Create(value);

        result.ShouldFailWith(SessionErrors.IpAddressRequired());
    }

    [TestMethod]
    [DataRow("999.999.999.999")]
    [DataRow("256.256.256.256")]
    [DataRow("192.168.1")]
    [DataRow("192.168.1.1.1")]
    [DataRow("abc.def.ghi.jkl")]
    [DataRow("invalid-ip")]
    public void Create_ShouldReturnFailure_WhenValueIsInvalid(string value)
    {
        var result = IPAddress.Create(value);

        result.ShouldFailWith(IPAddressErrors.IpAddressInvalid());
    }

    [TestMethod]
    [DataRow("127.0.0.1")]
    [DataRow("192.168.0.10")]
    [DataRow("8.8.8.8")]
    [DataRow("::1")]
    [DataRow("2001:db8::1")]
    [DataRow("   127.0.0.1   ")]
    public void Create_ShouldReturnSuccess_WhenValueIsValid(string value)
    {
        var result = IPAddress.Create(value);

        result.ShouldSucceed();
    }

    [TestMethod]
    public void Create_ShouldNormalizeWhitespace()
    {
        var result = IPAddress.Create("   192.168.1.100   ");

        result.ShouldSucceed();

        Assert.AreEqual("192.168.1.100", result.Value.Value);
    }

    [TestMethod]
    public void ToString_ShouldReturnValue()
    {
        var result = IPAddress.Create("192.168.1.100");

        result.ShouldSucceed();

        Assert.AreEqual("192.168.1.100", result.Value.ToString());
    }
}