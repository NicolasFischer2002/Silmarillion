using Domain.Policies;
using SharedKernel.Results;
using SharedKernel.Text;

namespace Domain.ValueObjects
{
    public sealed record AddressZipCode
    {
        public string Value { get; }

        private AddressZipCode(string value)
        {
            Value = value;
        }

        public static Result<AddressZipCode> Create(string value)
        {
            var normalizedValue = StringNormalizer.Normalize(value);
            var validation = AddressZipCodePolicy.Validate(normalizedValue);

            if (validation.IsFailure)
                return Result<AddressZipCode>.Failure(validation.Errors);

            return Result<AddressZipCode>.Success(new AddressZipCode(normalizedValue!));
        }

        public string GetFormattedValue()
        {
            return $"{Value[..5]}-{Value[5..]}";
        }

        public override string ToString() => Value;
    }
}