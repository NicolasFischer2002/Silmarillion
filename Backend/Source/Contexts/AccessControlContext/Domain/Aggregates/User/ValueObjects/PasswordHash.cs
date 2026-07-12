using Domain.Aggregates.User.Errors;
using SharedKernel.Results;

namespace Domain.Aggregates.User.ValueObjects
{
    public sealed record PasswordHash
    {
        public string Value { get; }

        private PasswordHash(string value)
        {
            Value = value;
        }

        public static Result<PasswordHash> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result<PasswordHash>.Failure(UserErrors.PasswordHashRequired());

            var normalizedValue = value.Trim();

            return Result<PasswordHash>.Success(new PasswordHash(normalizedValue));
        }

        public override string ToString() => "[PASSWORD_HASH]";
    }
}