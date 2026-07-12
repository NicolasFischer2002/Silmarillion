using SharedKernel.Results;

namespace Domain.Aggregates.Errors
{
    public static class IdentityErrors
    {
        // User
        public static Error UserIdInvalid()
            => Error.Validation("User.Id.Invalid", "User identifier is invalid.");

        public static Error UserAlreadyActive()
            => Error.BusinessRule("User.AlreadyActive", "User is already active.");

        public static Error UserAlreadyInactive()
            => Error.BusinessRule("User.AlreadyInactive", "User is already inactive.");

        public static Error UserAlreadyBlocked()
            => Error.BusinessRule("User.AlreadyBlocked", "User is already blocked.");

        public static Error UserMustBeActive()
            => Error.BusinessRule("User.MustBeActive", "User must be active to perform this action.");

        public static Error UserIsBlocked()
            => Error.BusinessRule("User.IsBlocked", "User is blocked.");

        // FullName
        public static Error FullNameRequired()
            => Error.Validation("User.FullName.Required", "Full name is required.");

        public static Error FullNameTooShort(int min)
            => Error.Validation("User.FullName.TooShort", $"Full name must be at least {min} characters long.");

        public static Error FullNameTooLong(int max)
            => Error.Validation("User.FullName.TooLong", $"Full name must be at most {max} characters long.");

        // Password
        public static Error PasswordIsRequired()
            => Error.Validation("User.Password.Required", "Password is required.");

        public static Error PasswordTooShort(int min)
            => Error.Validation("User.Password.TooShort", $"Password must be at least {min} characters long.");

        public static Error PasswordTooLong(int max)
            => Error.Validation("User.Password.TooLong", $"Password must be at most {max} characters long.");

        public static Error PasswordMissingUppercase()
            => Error.Validation("User.Password.MissingUppercase", "Password must contain at least one uppercase letter.");

        public static Error PasswordMissingLowercase()
            => Error.Validation("User.Password.MissingLowercase", "Password must contain at least one lowercase letter.");

        public static Error PasswordMissingNumber()
            => Error.Validation("User.Password.MissingNumber", "Password must contain at least one number.");

        public static Error PasswordMissingSpecialCharacter()
            => Error.Validation("User.Password.MissingSpecialCharacter", "Password must contain at least one special character.");

        public static Error PasswordContainsForbiddenValue()
            => Error.Validation("User.Password.ContainsForbiddenValue", "Password contains forbidden values.");

        public static Error PasswordHashRequired()
            => Error.Validation("User.PasswordHash.Required", "Password hash is required.");

        public static Error PasswordHashInvalid()
            => Error.Validation("User.PasswordHash.Invalid", "Password hash is invalid.");

        public static Error PasswordChangeRequired()
            => Error.BusinessRule("User.PasswordChangeRequired", "Password change is required.");
    }
}