using Domain.Errors;
using SharedKernel.Results;

namespace Domain.ValueObjects
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
                return Result<PasswordHash>.Failure(IdentityErrors.PasswordHashRequired());

            var normalizedValue = value.Trim();

            return Result<PasswordHash>.Success(new PasswordHash(normalizedValue));
        }

        public override string ToString() => "[PASSWORD_HASH]";
    }
}