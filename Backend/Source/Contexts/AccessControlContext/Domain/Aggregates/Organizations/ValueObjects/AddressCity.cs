using Domain.Aggregates.Organizations.Errors;
using SharedKernel.Results;
using SharedKernel.Text;

namespace Domain.Aggregates.Organizations.ValueObjects
{
    public sealed record AddressCity
    {
        public string Value { get; }
        private const int MaxLength = 100;
        private const int MinLength = 3;

        private AddressCity(string value)
        {
            Value = value;
        }

        public static Result<AddressCity> Create(string? value)
        {
            var normalizedValue = StringNormalizer.Normalize(value);

            if (string.IsNullOrWhiteSpace(normalizedValue))
                return Result<AddressCity>.Failure(OrganizationErrors.CityRequired());

            if (normalizedValue.Length < MinLength)
                return Result<AddressCity>.Failure(OrganizationErrors.CityTooShort(MinLength));

            if (normalizedValue.Length > MaxLength)
                return Result<AddressCity>.Failure(OrganizationErrors.CityTooLong(MaxLength));

            return Result<AddressCity>.Success(new AddressCity(normalizedValue));
        }

        public override string ToString() => Value;
    }
}