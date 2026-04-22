using Domain.Errors;
using SharedKernel.Results;
using SharedKernel.Text;

namespace Domain.Policies
{
    namespace Domain.Policies
    {
        public static class EmailAddressPolicy
        {
            public const int MaxLength = 254;

            public static Result<string> Validate(string? value)
            {
                var normalized = StringNormalizer.Normalize(value);

                if (string.IsNullOrWhiteSpace(normalized))
                    return Result<string>.Failure(IdentityErrors.EmailRequired());

                normalized = normalized.ToLowerInvariant();

                if (normalized.Length > MaxLength)
                    return Result<string>.Failure(IdentityErrors.EmailTooLong(MaxLength));

                if (!IsValidFormat(normalized))
                    return Result<string>.Failure(IdentityErrors.EmailInvalidFormat());

                return Result<string>.Success(normalized);
            }

            private static bool IsValidFormat(string email)
            {
                var parts = email.Split('@');

                if (parts.Length != 2)
                    return false;

                var local = parts[0];
                var domain = parts[1];

                if (string.IsNullOrWhiteSpace(local))
                    return false;

                if (string.IsNullOrWhiteSpace(domain))
                    return false;

                if (!domain.Contains('.'))
                    return false;

                return true;
            }
        }
    }
}