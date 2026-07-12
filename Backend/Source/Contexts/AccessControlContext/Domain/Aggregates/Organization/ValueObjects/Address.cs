using Domain.Aggregates.Organization.Constants;
using Domain.Aggregates.Organization.Errors;
using SharedKernel.Results;
using SharedKernel.Validation;

namespace Domain.Aggregates.Organization.ValueObjects
{
    public sealed record Address
    {
        public AddressState AddressState { get; }
        public AddressCity City { get; }
        public AddressZipCode ZipCode { get; }
        public AddressStreet Street { get; }
        public AddressNumber Number { get; }
        public AddressComplement Complement { get; }

        private Address(
            AddressState addressState, 
            AddressCity city,
            AddressZipCode zipCode,
            AddressStreet street, 
            AddressNumber number, 
            AddressComplement complement)
        {
            AddressState = addressState;
            City = city;
            ZipCode = zipCode;
            Street = street;
            Number = number;
            Complement = complement;
        }

        public static Result<Address> Create(
            AddressState addressState, 
            AddressCity city, 
            AddressZipCode zipCode,
            AddressStreet street, 
            AddressNumber number, 
            AddressComplement complement)
        {
            if (!EnumGuard.IsDefined(addressState))
                return Result<Address>.Failure(OrganizationErrors.InvalidState());

            if (city is null)
                return Result<Address>.Failure(OrganizationErrors.CityRequired());

            if (zipCode is null)
                return Result<Address>.Failure(OrganizationErrors.ZipCodeRequired());

            if (street is null)
                return Result<Address>.Failure(OrganizationErrors.StreetRequired());

            if (number is null)
                return Result<Address>.Failure(OrganizationErrors.NumberRequired());

            if (complement is null)
                return Result<Address>.Failure(OrganizationErrors.ComplementRequired());

            return Result<Address>.Success(new Address(
                addressState, city, zipCode, street, number, complement
            ));
        }
    }
}