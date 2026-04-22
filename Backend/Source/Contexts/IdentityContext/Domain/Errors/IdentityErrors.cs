using SharedKernel.Results;

namespace Domain.Errors
{
    public static class IdentityErrors
    {
        // User
        public static Error UserIdInvalid()
            => Error.Validation("User.Id.Invalid", "O identificador do usuário é inválido.");

        public static Error UserAlreadyActive()
            => Error.BusinessRule("User.AlreadyActive", "O usuário já está ativo.");

        public static Error UserAlreadyInactive()
            => Error.BusinessRule("User.AlreadyInactive", "O usuário já está inativo.");

        public static Error UserAlreadyBlocked()
            => Error.BusinessRule("User.AlreadyBlocked", "O usuário já está bloqueado.");

        public static Error UserMustBeActive()
            => Error.BusinessRule("User.MustBeActive", "O usuário precisa estar ativo para executar esta ação.");

        public static Error UserIsBlocked()
            => Error.BusinessRule("User.IsBlocked", "O usuário está bloqueado.");

        // FullName
        public static Error FullNameRequired()
            => Error.Validation("User.FullName.Required", "O nome é obrigatório.");

        public static Error FullNameTooShort(int min)
            => Error.Validation("User.FullName.TooShort", $"O nome deve ter pelo menos {min} caracteres.");

        public static Error FullNameTooLong(int max)
            => Error.Validation("User.FullName.TooLong", $"O nome deve ter no máximo {max} caracteres.");

        // Email
        public static Error EmailRequired()
            => Error.Validation("User.Email.Required", "O email é obrigatório.");

        public static Error EmailInvalid()
            => Error.Validation("User.Email.Invalid", "O email informado é inválido.");

        public static Error EmailTooLong(int maxLength)
            => Error.Validation("User.Email.TooLong", $"O email deve ter no máximo {maxLength} caracteres.");

        public static Error EmailInvalidFormat()
            => Error.Validation("User.Email.InvalidFormat", "O formato do email é inválido.");

        // Password
        public static Error PasswordIsRequired()
            => Error.Validation("User.Password.Required", "A senha é obrigatória.");

        public static Error PasswordTooShort(int min)
            => Error.Validation("User.Password.TooShort", $"A senha deve ter pelo menos {min} caracteres.");

        public static Error PasswordTooLong(int max)
            => Error.Validation("User.Password.TooLong", $"A senha deve ter no máximo {max} caracteres.");

        public static Error PasswordMissingUppercase()
            => Error.Validation("User.Password.MissingUppercase", "A senha deve conter ao menos uma letra maiúscula.");

        public static Error PasswordMissingLowercase()
            => Error.Validation("User.Password.MissingLowercase", "A senha deve conter ao menos uma letra minúscula.");

        public static Error PasswordMissingNumber()
            => Error.Validation("User.Password.MissingNumber", "A senha deve conter ao menos um número.");

        public static Error PasswordMissingSpecialCharacter()
            => Error.Validation("User.Password.MissingSpecialCharacter", "A senha deve conter ao menos um caractere especial.");

        public static Error PasswordContainsForbiddenValue()
            => Error.Validation("User.Password.ContainsForbiddenValue", "A senha contém valores proibidos.");

        public static Error PasswordHashRequired()
            => Error.Validation("User.PasswordHash.Required", "O hash da senha é obrigatório.");

        public static Error PasswordHashInvalid()
            => Error.Validation("User.PasswordHash.Invalid", "O hash da senha é inválido.");

        public static Error PasswordChangeRequired()
            => Error.BusinessRule("User.PasswordChangeRequired", "É necessário alterar a senha.");
    }
}