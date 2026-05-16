using Domain.Errors;
using SharedKernel.Results;
using SharedKernel.Text;

namespace Domain.ValueObjects
{
    public sealed record AddressComplement
    {
        public string Value { get; }
        private const int MaxLength = 100;

        private AddressComplement(string value)
        {
            Value = value;
        }

        public static Result<AddressComplement> Create(string? value)
        {
            var normalizedValue = StringNormalizer.NormalizeOrEmpty(value);

            if (normalizedValue.Length > MaxLength)
                return Result<AddressComplement>.Failure(OrganizationErrors.ComplementTooLong(MaxLength));

            return Result<AddressComplement>.Success(new AddressComplement(normalizedValue));
        }

        public override string ToString() => Value;
    }
}