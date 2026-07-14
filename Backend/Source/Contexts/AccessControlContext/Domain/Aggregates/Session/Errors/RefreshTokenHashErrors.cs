using SharedKernel.Results;

namespace Domain.Aggregates.Session.Errors
{
    public static class RefreshTokenHashErrors
    {
        public static Error RefreshTokenHashTooLong(int maxLength)
            => Error.Validation(
                "RefreshTokenHash.TooLong",
                $"Refresh token hash must not exceed {maxLength} characters.");
    }
}