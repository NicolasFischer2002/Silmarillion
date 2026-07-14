using Domain.Aggregates.Session.Errors;
using SharedKernel.Results;
using SharedKernel.Text;

namespace Domain.Aggregates.Session.ValueObjects
{
    public sealed record UserAgent
    {
        public string Value { get; }

        private const int MaxLength = 512;

        private UserAgent(string value)
        {
            Value = value;
        }

        public static Result<UserAgent> Create(string? value)
        {
            var normalizedValue = StringNormalizer.Normalize(value);

            if (string.IsNullOrWhiteSpace(normalizedValue))
                return Result<UserAgent>.Failure(UserAgentErrors.UserAgentRequired());

            if (normalizedValue.Length > MaxLength)
                return Result<UserAgent>.Failure(UserAgentErrors.UserAgentTooLong(MaxLength));

            return Result<UserAgent>.Success(
                new UserAgent(normalizedValue));
        }

        public override string ToString() => Value;
    }
}