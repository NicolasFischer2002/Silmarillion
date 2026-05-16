using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.ValueObjects;
using SharedKernel.Errors;
using SharedTests.Assertions;

namespace Tests.Domain.Entities;

[TestClass]
public class OrganizationTests
{
    private static OrganizationName ValidName()
        => OrganizationName.Create("Empresa Exemplo Ltda").Value;

    private static Cnpj ValidCnpj()
        => Cnpj.Create("11222333000181").Value;

    private static EmailAddress ValidEmail()
        => EmailAddress.Create("contato@empresa.com").Value;

    private static Address ValidAddress()
        => Address.Create(
            AddressState.SP,
            AddressCity.Create("São Paulo").Value,
            AddressZipCode.Create("12345678").Value,
            AddressStreet.Create("Rua das Flores").Value,
            AddressNumber.Create("123").Value,
            AddressComplement.Create("Apto 101").Value
        ).Value;

    [TestMethod]
    public void Create_ShouldReturnFailure_WhenIdIsEmpty()
    {
        var result = Organization.Create(
            Guid.Empty,
            ValidName(),
            ValidCnpj(),
            ValidEmail(),
            ValidAddress()
        );

        result.ShouldFailWith(OrganizationErrors.OrganizationIdInvalid());
    }

    [TestMethod]
    public void Create_ShouldReturnFailure_WhenNameIsNull()
    {
        var result = Organization.Create(
            Guid.NewGuid(),
            null!,
            ValidCnpj(),
            ValidEmail(),
            ValidAddress()
        );

        result.ShouldFailWith(OrganizationErrors.OrganizationNameRequired());
    }

    [TestMethod]
    public void Create_ShouldReturnFailure_WhenCnpjIsNull()
    {
        var result = Organization.Create(
            Guid.NewGuid(),
            ValidName(),
            null!,
            ValidEmail(),
            ValidAddress()
        );

        result.ShouldFailWith(OrganizationErrors.CnpjRequired());
    }

    [TestMethod]
    public void Create_ShouldReturnFailure_WhenEmailIsNull()
    {
        var result = Organization.Create(
            Guid.NewGuid(),
            ValidName(),
            ValidCnpj(),
            null!,
            ValidAddress()
        );

        result.ShouldFailWith(EmailAddressPolicyErrors.EmailRequired());
    }

    [TestMethod]
    public void Create_ShouldReturnFailure_WhenAddressIsNull()
    {
        var result = Organization.Create(
            Guid.NewGuid(),
            ValidName(),
            ValidCnpj(),
            ValidEmail(),
            null!
        );

        result.ShouldFailWith(OrganizationErrors.AddressRequired());
    }

    [TestMethod]
    public void Create_ShouldReturnSuccess_WhenInputIsValid()
    {
        var id = Guid.NewGuid();
        var name = ValidName();
        var cnpj = ValidCnpj();
        var email = ValidEmail();
        var address = ValidAddress();

        var result = Organization.Create(
            id,
            name,
            cnpj,
            email,
            address
        );

        result.ShouldSucceed();

        var organization = result.Value;

        Assert.AreEqual(id, organization.Id);
        Assert.AreEqual(name, organization.OrganizationName);
        Assert.AreEqual(cnpj, organization.Cnpj);
        Assert.AreEqual(email, organization.EmailAddress);
        Assert.AreEqual(address, organization.Address);

        Assert.AreEqual(OrganizationStatus.Active, organization.OrganizationStatus);

        Assert.IsTrue(organization.CreatedAt <= DateTime.UtcNow);
        Assert.IsTrue(organization.LastModifiedAt <= DateTime.UtcNow);
    }
}