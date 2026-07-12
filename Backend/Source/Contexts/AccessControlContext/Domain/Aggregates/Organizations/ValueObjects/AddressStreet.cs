using Domain.Aggregates.Organizations.Errors;
using SharedKernel.Results;
using SharedKernel.Text;

namespace Domain.Aggregates.Organizations.ValueObjects
{
    public sealed record AddressStreet
    {
        public string Value { get; }
        private const int MaxLength = 100;
        private const int MinLength = 1;

        private AddressStreet(string value)
        {
            Value = value;
        }

        public static Result<AddressStreet> Create(string? value)
        {
            var normalizedValue = StringNormalizer.Normalize(value);

            if (string.IsNullOrWhiteSpace(normalizedValue))
                return Result<AddressStreet>.Failure(OrganizationErrors.StreetRequired());

            if (normalizedValue.Length < MinLength)
                return Result<AddressStreet>.Failure(OrganizationErrors.StreetTooShort(MinLength));

            if (normalizedValue.Length > MaxLength)
                return Result<AddressStreet>.Failure(OrganizationErrors.StreetTooLong(MaxLength));

            return Result<AddressStreet>.Success(new AddressStreet(normalizedValue));
        }

        public override string ToString() => Value;
    }
}