using SharedKernel.Results;

namespace Domain.Aggregates.Session.Errors
{
    public static class UserAgentErrors
    {
        public static Error UserAgentRequired()
           => Error.Validation(
           "Session.UserAgent.Required",
           "User agent is required.");

        public static Error UserAgentTooLong(int max)
            => Error.Validation(
                "Session.UserAgent.TooLong",
                $"User agent must be at most {max} characters long.");
    }
}