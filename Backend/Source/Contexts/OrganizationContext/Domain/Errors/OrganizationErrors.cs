using SharedKernel.Results;

namespace Domain.Errors
{
    internal static class OrganizationErrors
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

        // OrganizationName
        public static Error OrganizationNameRequired()
            => Error.Validation("Organization.OrganizationName.Required", "O nome da organização é obrigatório.");

        public static Error OrganizationNameTooShort(int minLength)
            => Error.Validation("Organization.OrganizationName.TooShort", $"O nome da organização deve ter no mínimo {minLength} caracteres.");

        public static Error OrganizationNameTooLong(int maxLength)
            => Error.Validation("Organization.OrganizationName.TooLong", $"O nome da organização deve ter no máximo {maxLength} caracteres.");

        // Cnpj
        public static Error CnpjRequired()
            => Error.Validation("Organization.Cnpj.Required", "O CNPJ é obrigatório.");

        public static Error CnpjInvalid()
            => Error.Validation("Organization.Cnpj.Invalid", "O CNPJ informado é inválido.");

        // City
        public static Error CityRequired()
            => Error.Validation("Organization.Address.City.Required", "A cidade é obrigatória.");

        public static Error CityTooLong(int maxLength)
            => Error.Validation("Organization.Address.City.TooLong", $"A cidade deve ter no máximo {maxLength} caracteres.");

        public static Error CityTooShort(int minLength)
            => Error.Validation("Organization.Address.City.TooShort", $"A cidade deve ter no mínimo {minLength} caracteres.");

        // ZipCode
        public static Error ZipCodeRequired()
            => Error.Validation("Organization.Address.ZipCode.Required", "O CEP é obrigatório.");

        public static Error ZipCodeInvalid()
            => Error.Validation("Organization.Address.ZipCode.Invalid", "O CEP informado é inválido.");

        // Street
        public static Error StreetRequired()
            => Error.Validation("Organization.Address.Street.Required", "A rua é obrigatória.");

        // Number
        public static Error NumberRequired()
            => Error.Validation("Organization.Address.Number.Required", "O número é obrigatório.");

        // Complement
        public static Error ComplementRequired()
            => Error.Validation("Organization.Address.Complement.Required", "O complemento é obrigatório.");

        // State
        public static Error InvalidState()
            => Error.Validation("Organization.Address.State.Invalid", "O estado informado é inválido.");
    }
}