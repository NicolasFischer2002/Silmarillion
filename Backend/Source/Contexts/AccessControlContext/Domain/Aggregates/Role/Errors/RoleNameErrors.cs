using SharedKernel.Results;

namespace Domain.Aggregates.Role.Errors
{
    public static class RoleNameErrors
    {
        public static Error NameRequired() =>
            Error.Validation(
                code: "RoleName.Required",
                message: "The role name is mandatory.");

        public static Error NameTooShort(int minLength) =>
            Error.Validation(
                code: "RoleName.TooShort",
                message: $"The role name must contain at least {minLength} characters.");

        public static Error NameTooLong(int maxLength) =>
            Error.Validation(
                code: "RoleName.TooLong",
                message: $"The role name cannot contain more than {maxLength} characters.");
    }
}