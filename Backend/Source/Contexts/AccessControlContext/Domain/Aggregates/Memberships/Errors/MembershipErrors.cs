using SharedKernel.Results;

namespace Domain.Aggregates.Memberships.Errors
{
    public static class MembershipErrors
    {
        public static Error MembershipIdRequired() =>
            Error.Validation(
                "Membership.Id.Required", 
                "O identificador da associação é obrigatório.");

        public static Error UserIdRequired() =>
            Error.Validation(
                "Membership.UserId.Required", 
                "O identificador do usuário é obrigatório.");

        public static Error OrganizationIdRequired() =>
            Error.Validation(
                "Membership.OrganizationId.Required", 
                "O identificador da organização é obrigatório.");

        public static Error MembershipAlreadyActive() =>
            Error.BusinessRule(
                "Membership.AlreadyActive", 
                "A associação já está ativa.");

        public static Error MembershipAlreadySuspended() =>
            Error.BusinessRule(
                "Membership.AlreadySuspended",
                "A associação já está suspensa.");

        public static Error CannotActivateRevokedMembership() =>
            Error.BusinessRule(
                "Membership.CannotActivateRevoked",
                "Não é possível ativar uma associação revogada.");

        public static Error MembershipAlreadyInactive() =>
            Error.BusinessRule(
                "Membership.AlreadyInactive",
                "A associação já está inativa.");

        public static Error CannotDeactivateRevokedMembership() =>
            Error.BusinessRule(
                "Membership.CannotDeactivateRevoked",
                "Não é possível desativar uma associação revogada.");

        public static Error MembershipAlreadyRevoked() =>
            Error.BusinessRule(
                "Membership.AlreadyRevoked",
                "A associação já está revogada.");

        public static Error RoleIdRequired() =>
            Error.Validation(
            code: "Membership.RoleIdRequired",
            message: "O id da função é obrigatório.");

        public static Error RoleAlreadyAssigned() =>
            Error.Conflict(
                code: "Membership.RoleAlreadyAssigned",
                message: "A função já está atribuída à associação.");

        public static Error RoleNotAssigned() =>
            Error.Validation(
                code: "Membership.RoleNotAssigned",
                message: "A função não está atribuída à associação.");

        public static Error MembershipNotFound() =>
            Error.NotFound(
                "Membership.NotFound",
                "Não foi possível encontrar nenhum membro.");
    }
}