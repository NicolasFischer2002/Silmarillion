using SharedKernel.Results;

namespace Domain.Aggregates.Organizations.Errors
{
    public static class OrganizationErrors
    {
        // Organization
        public static Error OrganizationIdInvalid()
            => Error.Validation("Organization.Id.Invalid", "Organization identifier is invalid.");

        // OrganizationName
        public static Error OrganizationNameRequired()
            => Error.Validation("Organization.OrganizationName.Required", "Organization name is required.");

        public static Error OrganizationNameTooShort(int minLength)
            => Error.Validation(
                "Organization.OrganizationName.TooShort",
                $"Organization name must be at least {minLength} characters long.");

        public static Error OrganizationNameTooLong(int maxLength)
            => Error.Validation(
                "Organization.OrganizationName.TooLong",
                $"Organization name must be at most {maxLength} characters long.");

        // Cnpj
        public static Error CnpjRequired()
            => Error.Validation("Organization.Cnpj.Required", "CNPJ is required.");

        public static Error CnpjInvalid()
            => Error.Validation("Organization.Cnpj.Invalid", "Provided CNPJ is invalid.");

        // City
        public static Error CityRequired()
            => Error.Validation("Organization.Address.City.Required", "City is required.");

        public static Error CityTooLong(int maxLength)
            => Error.Validation(
                "Organization.Address.City.TooLong",
                $"City must be at most {maxLength} characters long.");

        public static Error CityTooShort(int minLength)
            => Error.Validation(
                "Organization.Address.City.TooShort",
                $"City must be at least {minLength} characters long.");

        // ZipCode
        public static Error ZipCodeRequired()
            => Error.Validation("Organization.Address.ZipCode.Required", "Zip code is required.");

        public static Error ZipCodeInvalid()
            => Error.Validation("Organization.Address.ZipCode.Invalid", "Provided zip code is invalid.");

        // Street
        public static Error StreetRequired()
            => Error.Validation("Organization.Address.Street.Required", "Street is required.");

        public static Error StreetTooShort(int minLength)
            => Error.Validation(
                "Organization.Address.Street.TooShort",
                $"Street must be at least {minLength} characters long.");

        public static Error StreetTooLong(int maxLength)
            => Error.Validation(
                "Organization.Address.Street.TooLong",
                $"Street must be at most {maxLength} characters long.");

        // Number
        public static Error NumberRequired()
            => Error.Validation("Organization.Address.Number.Required", "Address number is required.");

        public static Error NumberTooShort(int minLength)
            => Error.Validation(
                "Organization.Address.Number.TooShort",
                $"Address number must be at least {minLength} characters long.");

        public static Error NumberTooLong(int maxLength)
            => Error.Validation(
                "Organization.Address.Number.TooLong",
                $"Address number must be at most {maxLength} characters long.");

        // Complement
        public static Error ComplementRequired()
            => Error.Validation("Organization.Address.Complement.Required", "Address complement is required.");

        public static Error ComplementTooLong(int maxLength)
            => Error.Validation(
                "Organization.Address.Complement.TooLong",
                $"Address complement must be at most {maxLength} characters long.");

        // State
        public static Error InvalidState()
            => Error.Validation("Organization.Address.State.Invalid", "Provided state is invalid.");

        // Address
        public static Error AddressRequired()
            => Error.Validation("Organization.Address.Required", "Address is required.");
    }
}