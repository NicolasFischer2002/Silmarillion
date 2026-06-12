using SharedKernel.Results;

namespace Domain.Aggregates.Memberships.Errors
{
    public static class MembershipErrors
    {
        public static Error MembershipIdRequired() =>
            Error.Validation(
                "Membership.Id.Required",
                "Membership identifier is required.");

        public static Error UserIdRequired() =>
            Error.Validation(
                "Membership.UserId.Required",
                "User identifier is required.");

        public static Error OrganizationIdRequired() =>
            Error.Validation(
                "Membership.OrganizationId.Required",
                "Organization identifier is required.");

        public static Error MembershipAlreadyActive() =>
            Error.BusinessRule(
                "Membership.AlreadyActive",
                "Membership is already active.");

        public static Error MembershipAlreadySuspended() =>
            Error.BusinessRule(
                "Membership.AlreadySuspended",
                "Membership is already suspended.");

        public static Error CannotActivateRevokedMembership() =>
            Error.BusinessRule(
                "Membership.CannotActivateRevoked",
                "Cannot activate a revoked membership.");

        public static Error MembershipAlreadyInactive() =>
            Error.BusinessRule(
                "Membership.AlreadyInactive",
                "Membership is already inactive.");

        public static Error CannotDeactivateRevokedMembership() =>
            Error.BusinessRule(
                "Membership.CannotDeactivateRevoked",
                "Cannot deactivate a revoked membership.");

        public static Error MembershipAlreadyRevoked() =>
            Error.BusinessRule(
                "Membership.AlreadyRevoked",
                "Membership is already revoked.");

        public static Error RoleIdRequired() =>
            Error.Validation(
                code: "Membership.RoleIdRequired",
                message: "Role identifier is required.");

        public static Error RoleAlreadyAssigned() =>
            Error.Conflict(
                code: "Membership.RoleAlreadyAssigned",
                message: "Role is already assigned to the membership.");

        public static Error RoleNotAssigned() =>
            Error.Validation(
                code: "Membership.RoleNotAssigned",
                message: "Role is not assigned to the membership.");

        public static Error MembershipNotFound() =>
            Error.NotFound(
                "Membership.NotFound",
                "Membership was not found.");
    }
}