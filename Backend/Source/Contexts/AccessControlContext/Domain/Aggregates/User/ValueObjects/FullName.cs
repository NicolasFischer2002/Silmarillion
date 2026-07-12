using Domain.Aggregates.User.Errors;
using SharedKernel.Results;
using SharedKernel.Text;

namespace Domain.Aggregates.User.ValueObjects
{
    public sealed record FullName
    {
        public string Value { get; }

        private const int MaxLength = 100;
        private const int MinLength = 10;

        private FullName(string value)
        {
            Value = value;
        }

        public static Result<FullName> Create(string? value)
        {
            var normalizedValue = StringNormalizer.Normalize(value);

            if (string.IsNullOrWhiteSpace(normalizedValue))
                return Result<FullName>.Failure(UserErrors.FullNameRequired());

            if (normalizedValue.Length < MinLength)
                return Result<FullName>.Failure(UserErrors.FullNameTooShort(MinLength));

            if (normalizedValue.Length > MaxLength)
                return Result<FullName>.Failure(UserErrors.FullNameTooLong(MaxLength));

            return Result<FullName>.Success(new FullName(normalizedValue));
        }

        public override string ToString() => Value;
    }
}