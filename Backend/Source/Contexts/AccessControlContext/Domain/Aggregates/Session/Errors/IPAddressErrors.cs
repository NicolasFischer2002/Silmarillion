using SharedKernel.Results;

namespace Domain.Aggregates.Session.Errors
{
    public static class IPAddressErrors
    {
        public static Error IpAddressRequired()
            => Error.Validation(
                "Session.IPAddress.Required",
                "IP address is required.");

        public static Error IpAddressTooLong(int maxLength)
            => Error.Validation(
                "Session.IpAddress.TooLong",
                $"IP address must not exceed {maxLength} characters.");

        public static Error IpAddressInvalid()
            => Error.Validation(
                "Session.IpAddress.Invalid",
                "IP address is invalid.");
    }
}