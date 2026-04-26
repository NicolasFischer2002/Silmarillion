using Domain.Policies;
using SharedKernel.Results;

namespace Domain.ValueObjects
{
    public sealed record EmailAddress
    {
        public string Value { get; }

        private EmailAddress(string value)
        {
            Value = value;
        }

        public static Result<EmailAddress> Create(string? value)
        {
            var validation = EmailAddressPolicy.Validate(value);

            if (validation.IsFailure)
                return Result<EmailAddress>.Failure(validation.Errors);

            return Result<EmailAddress>.Success(
                new EmailAddress(validation.Value)
            );
        }

        public override string ToString() => Value;
    }
}