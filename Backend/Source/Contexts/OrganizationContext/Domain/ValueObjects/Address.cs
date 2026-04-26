using Domain.Enums;
using SharedKernel.Results;

namespace Domain.ValueObjects
{
    public sealed record Address
    {
        public AddressState AddressState { get; private set; }
        public AddressCity City { get; private set; }
        public AddressZipCode ZipCode { get; private set; }
        public AddressStreet Street { get; private set; }
        public AddressNumber Number { get; private set; }
        public AddressComplement Complement { get; private set; }

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
            return Result<Address>.Success(new Address(
                addressState, city, zipCode, street, number, complement
            ));
        }
    }
}