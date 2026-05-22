using SharedKernel.Results;

namespace Domain.Aggregates.Roles.Errors
{
    public static class RoleErrors
    {
        public static Error RoleIdRequired() =>
        Error.Validation(
            code: "Role.IdRequired",
            message: "O id da função é obrigatório.");

        public static Error OrganizationIdRequired() =>
            Error.Validation(
                code: "Role.OrganizationIdRequired",
                message: "O id da organização é obrigatório.");

        public static Error RoleAlreadyActive() =>
            Error.Conflict(
                code: "Role.AlreadyActive",
                message: "A função já está ativa.");

        public static Error RoleAlreadyInactive() =>
            Error.Conflict(
                code: "Role.AlreadyInactive",
                message: "A função já está inativa.");

        public static Error PermissionAlreadyAssigned() =>
            Error.Conflict(
                code: "Role.PermissionAlreadyAssigned",
                message: "A permissão já está atribuída à função.");

        public static Error PermissionNotAssigned() =>
            Error.Validation(
                code: "Role.PermissionNotAssigned", 
                message: "A permissão não está atribuída à função.");
    }
}