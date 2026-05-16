using SharedKernel.Results;

namespace SharedKernel.Errors
{
    public static class EmailAddressPolicyErrors
    {
        public static Error EmailRequired()
            => Error.Validation("Organization.Email.Required", "O email é obrigatório.");

        public static Error EmailInvalid()
            => Error.Validation("Organization.Email.Invalid", "O email informado é inválido.");

        public static Error EmailTooLong(int maxLength)
            => Error.Validation("Organization.Email.TooLong", $"O email deve ter no máximo {maxLength} caracteres.");

        public static Error EmailInvalidFormat()
            => Error.Validation("Organization.Email.InvalidFormat", "O formato do email é inválido.");
    }
}