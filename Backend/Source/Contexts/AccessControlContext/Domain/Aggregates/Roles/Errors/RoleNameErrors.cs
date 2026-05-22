using SharedKernel.Results;

namespace Domain.Aggregates.Roles.Errors
{
    public static class RoleNameErrors
    {
        public static Error NameRequired() =>
            Error.Validation(
                code: "RoleName.Required",
                message: "O nome da função é obrigatório.");

        public static Error NameTooShort(int minLength) =>
            Error.Validation(
                code: "RoleName.TooShort",
                message: $"O nome da função deve conter pelo menos {minLength} caracteres.");

        public static Error NameTooLong(int maxLength) =>
            Error.Validation(
                code: "RoleName.TooLong",
                message: $"O nome da função não pode conter mais de {maxLength} caracteres.");
    }
}