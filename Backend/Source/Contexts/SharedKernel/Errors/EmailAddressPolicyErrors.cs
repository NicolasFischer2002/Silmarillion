using SharedKernel.Results;

namespace SharedKernel.Errors
{
    public static class EmailAddressPolicyErrors
    {
        public static Error EmailRequired()
            => Error.Validation(
                "Organization.Email.Required", 
                "The email is mandatory.");

        public static Error EmailInvalid()
            => Error.Validation(
                "Organization.Email.Invalid", 
                "The provided email is invalid.");

        public static Error EmailTooLong(int maxLength)
            => Error.Validation(
                "Organization.Email.TooLong", 
                $"The email must contain at most {maxLength} characters.");

        public static Error EmailInvalidFormat()
            => Error.Validation(
                "Organization.Email.InvalidFormat", 
                "The email format is invalid.");
    }
}