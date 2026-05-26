using SharedKernel.Results;

namespace Domain.Errors
{
    public static class AccessControlErrors
    {
        public static Error RoleDoesNotBelongToOrganization() =>
            Error.BusinessRule(
                "AccessControl.Role.InvalidOrganization", 
                "Role does not belong to the membership organization."
            );
    }
}