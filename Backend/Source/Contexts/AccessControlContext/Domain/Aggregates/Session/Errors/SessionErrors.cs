using SharedKernel.Results;

namespace Domain.Aggregates.Session.Errors
{
    public static class SessionErrors
    {
        public static Error SessionIdInvalid()
            => Error.Validation(
                "Session.Id.Invalid",
                "Session identifier is invalid.");

        public static Error UserIdInvalid()
            => Error.Validation(
                "Session.UserId.Invalid",
                "User identifier is invalid.");

        public static Error RefreshTokenHashRequired()
            => Error.Validation(
                "Session.RefreshTokenHash.Required",
                "Refresh token hash is required.");

        public static Error SessionAlreadyRevoked()
            => Error.BusinessRule(
                "Session.AlreadyRevoked",
                "Session has already been revoked.");

        public static Error SessionExpired()
            => Error.BusinessRule(
                "Session.Expired",
                "Session has expired.");

        public static Error SessionNotActive()
            => Error.BusinessRule(
                "Session.NotActive",
                "Session is not active.");
    }
}