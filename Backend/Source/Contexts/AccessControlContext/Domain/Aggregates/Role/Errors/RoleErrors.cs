using SharedKernel.Results;

namespace Domain.Aggregates.Role.Errors
{
    public static class RoleErrors
    {
        public static Error RoleIdRequired() =>
        Error.Validation(
            code: "Role.IdRequired",
            message: "The role ID is mandatory.");

        public static Error OrganizationIdRequired() =>
            Error.Validation(
                code: "Role.OrganizationIdRequired",
                message: "The organization ID is mandatory.");

        public static Error RoleAlreadyActive() =>
            Error.Conflict(
                code: "Role.AlreadyActive",
                message: "The role is already active.");

        public static Error RoleAlreadyInactive() =>
            Error.Conflict(
                code: "Role.AlreadyInactive",
                message: "The role is already inactive.");

        public static Error PermissionAlreadyAssigned() =>
            Error.Conflict(
                code: "Role.PermissionAlreadyAssigned",
                message: "The permission is already assigned to the role.");

        public static Error PermissionNotAssigned() =>
            Error.Validation(
                code: "Role.PermissionNotAssigned",
                message: "The permission is not assigned to the role.");

        public static Error RoleNotFound() =>
            Error.NotFound(
                code: "Role.NotFound",
                message: "The role could not be found.");
    }
}