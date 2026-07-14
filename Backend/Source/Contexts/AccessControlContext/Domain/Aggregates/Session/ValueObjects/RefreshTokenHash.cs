using Domain.Aggregates.Session.Errors;
using SharedKernel.Results;
using SharedKernel.Text;

namespace Domain.Aggregates.Session.ValueObjects
{
    public sealed record RefreshTokenHash
    {
        public string Value { get; }

        private const int MaxLength = 512;

        private RefreshTokenHash(string value)
        {
            Value = value;
        }

        public static Result<RefreshTokenHash> Create(string? value)
        {
            var normalizedValue = StringNormalizer.Normalize(value);

            if (string.IsNullOrWhiteSpace(normalizedValue))
                return Result<RefreshTokenHash>.Failure(
                    SessionErrors.RefreshTokenHashRequired());

            if (normalizedValue.Length > MaxLength)
                return Result<RefreshTokenHash>.Failure(
                    RefreshTokenHashErrors.RefreshTokenHashTooLong(MaxLength));

            return Result<RefreshTokenHash>.Success(
                new RefreshTokenHash(normalizedValue));
        }

        public override string ToString() => Value;
    }
}