using Domain.Aggregates.Organization.Policies;
using SharedKernel.Results;
using SharedKernel.Text;

namespace Domain.Aggregates.Organization.ValueObjects
{
    public sealed record AddressZipCode
    {
        public string Value { get; }

        private AddressZipCode(string value)
        {
            Value = value;
        }

        public static Result<AddressZipCode> Create(string? value)
        {
            var normalizedValue = Normalize(StringNormalizer.Normalize(value));
            var validation = AddressZipCodePolicy.Validate(normalizedValue);

            if (validation.IsFailure)
                return Result<AddressZipCode>.Failure(validation.Errors);

            return Result<AddressZipCode>.Success(new AddressZipCode(normalizedValue!));
        }

        private static string? Normalize(string? value)
        {
            if (value is null)
                return null;

            var digitsOnly = new string([.. value.Where(char.IsDigit)]);

            return digitsOnly.Trim();
        }

        public string GetFormattedValue()
        {
            return $"{Value[..5]}-{Value[5..]}";
        }

        public override string ToString() => Value;
    }
}