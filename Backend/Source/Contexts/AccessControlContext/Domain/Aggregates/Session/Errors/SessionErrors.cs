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

        public static Error IpAddressRequired()
            => Error.Validation(
                "Session.IPAddress.Required",
                "IP address is required.");

        public static Error UserAgentRequired()
            => Error.Validation(
                "Session.UserAgent.Required",
                "User agent is required.");

        public static Error ExpirationDateInvalid()
            => Error.Validation(
                "Session.ExpirationDate.Invalid",
                "Expiration date is invalid.");

        public static Error SessionAlreadyExpired()
            => Error.BusinessRule(
                "Session.AlreadyExpired",
                "Session has already expired.");

        public static Error RefreshTokenAlreadyInUse()
            => Error.BusinessRule(
                "Session.RefreshToken.AlreadyInUse",
                "Refresh token hash is already in use.");
    }
}