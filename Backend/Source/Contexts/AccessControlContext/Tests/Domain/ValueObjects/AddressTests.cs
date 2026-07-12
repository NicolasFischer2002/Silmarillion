using Domain.Aggregates.Organizations.Constants;
using Domain.Aggregates.Organizations.Errors;
using Domain.Aggregates.Organizations.ValueObjects;
using SharedTests.Assertions;

namespace Tests.Domain.ValueObjects;

[TestClass]
public class AddressTests
{
    private static AddressState ValidState() => AddressState.SP;

    private static AddressCity ValidCity()
        => AddressCity.Create("São Paulo").Value;

    private static AddressZipCode ValidZipCode()
        => AddressZipCode.Create("12345678").Value;

    private static AddressStreet ValidStreet()
        => AddressStreet.Create("Rua das Flores").Value;

    private static AddressNumber ValidNumber()
        => AddressNumber.Create("123").Value;

    private static AddressComplement ValidComplement()
        => AddressComplement.Create("Apto 101").Value;

    [TestMethod]
    public void Create_ShouldReturnFailure_WhenStateIsInvalid()
    {
        var invalidState = (AddressState)999;

        var result = Address.Create(
            invalidState,
            ValidCity(),
            ValidZipCode(),
            ValidStreet(),
            ValidNumber(),
            ValidComplement()
        );

        result.ShouldFailWith(OrganizationErrors.InvalidState());
    }

    [TestMethod]
    public void Create_ShouldReturnFailure_WhenCityIsNull()
    {
        var result = Address.Create(
            ValidState(),
            null!,
            ValidZipCode(),
            ValidStreet(),
            ValidNumber(),
            ValidComplement()
        );

        result.ShouldFailWith(OrganizationErrors.CityRequired());
    }

    [TestMethod]
    public void Create_ShouldReturnFailure_WhenZipCodeIsNull()
    {
        var result = Address.Create(
            ValidState(),
            ValidCity(),
            null!,
            ValidStreet(),
            ValidNumber(),
            ValidComplement()
        );

        result.ShouldFailWith(OrganizationErrors.ZipCodeRequired());
    }

    [TestMethod]
    public void Create_ShouldReturnFailure_WhenStreetIsNull()
    {
        var result = Address.Create(
            ValidState(),
            ValidCity(),
            ValidZipCode(),
            null!,
            ValidNumber(),
            ValidComplement()
        );

        result.ShouldFailWith(OrganizationErrors.StreetRequired());
    }

    [TestMethod]
    public void Create_ShouldReturnFailure_WhenNumberIsNull()
    {
        var result = Address.Create(
            ValidState(),
            ValidCity(),
            ValidZipCode(),
            ValidStreet(),
            null!,
            ValidComplement()
        );

        result.ShouldFailWith(OrganizationErrors.NumberRequired());
    }

    [TestMethod]
    public void Create_ShouldReturnFailure_WhenComplementIsNull()
    {
        var result = Address.Create(
            ValidState(),
            ValidCity(),
            ValidZipCode(),
            ValidStreet(),
            ValidNumber(),
            null!
        );

        result.ShouldFailWith(OrganizationErrors.ComplementRequired());
    }

    [TestMethod]
    public void Create_ShouldReturnSuccess_WhenAllValuesAreValid()
    {
        var state = ValidState();
        var city = ValidCity();
        var zipCode = ValidZipCode();
        var street = ValidStreet();
        var number = ValidNumber();
        var complement = ValidComplement();

        var result = Address.Create(
            state,
            city,
            zipCode,
            street,
            number,
            complement
        );

        result.ShouldSucceed();

        var address = result.Value;

        Assert.AreEqual(state, address.AddressState);
        Assert.AreEqual(city, address.City);
        Assert.AreEqual(zipCode, address.ZipCode);
        Assert.AreEqual(street, address.Street);
        Assert.AreEqual(number, address.Number);
        Assert.AreEqual(complement, address.Complement);
    }
}