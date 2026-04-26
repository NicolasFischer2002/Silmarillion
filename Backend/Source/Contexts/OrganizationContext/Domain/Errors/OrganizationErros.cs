using SharedKernel.Results;

namespace Domain.Errors
{
    internal static class OrganizationErros
    {
        // Email
        public static Error EmailRequired()
            => Error.Validation("User.Email.Required", "O email é obrigatório.");

        public static Error EmailInvalid()
            => Error.Validation("User.Email.Invalid", "O email informado é inválido.");

        public static Error EmailTooLong(int maxLength)
            => Error.Validation("User.Email.TooLong", $"O email deve ter no máximo {maxLength} caracteres.");

        public static Error EmailInvalidFormat()
            => Error.Validation("User.Email.InvalidFormat", "O formato do email é inválido.");
    }
}