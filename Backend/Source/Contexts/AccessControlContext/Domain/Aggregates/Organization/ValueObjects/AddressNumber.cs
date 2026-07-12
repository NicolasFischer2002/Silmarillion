using Domain.Aggregates.Organization.Errors;
using SharedKernel.Results;
using SharedKernel.Text;

namespace Domain.Aggregates.Organization.ValueObjects
{
    public sealed record AddressNumber
    {
        public string Value { get; }
        private const int MaxLength = 10;
        private const int MinLength = 1;

        private AddressNumber(string value)
        {
            Value = value;
        }

        public static Result<AddressNumber> Create(string? value)
        {
            var normalizedValue = StringNormalizer.Normalize(value);

            if (string.IsNullOrWhiteSpace(normalizedValue))
                return Result<AddressNumber>.Failure(OrganizationErrors.NumberRequired());

            if (normalizedValue.Length < MinLength)
                return Result<AddressNumber>.Failure(OrganizationErrors.NumberTooShort(MinLength));

            if (normalizedValue.Length > MaxLength)
                return Result<AddressNumber>.Failure(OrganizationErrors.NumberTooLong(MaxLength));

            return Result<AddressNumber>.Success(new AddressNumber(normalizedValue));
        }

        public override string ToString() => Value;
    }
}