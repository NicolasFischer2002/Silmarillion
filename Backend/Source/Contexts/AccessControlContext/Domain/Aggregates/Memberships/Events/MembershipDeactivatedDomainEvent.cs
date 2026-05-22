using SharedKernel.DomainEvents;

namespace Domain.Aggregates.Memberships.Events
{
    public sealed record MembershipDeactivatedDomainEvent(
        Guid MembershipId,
        Guid UserId,
        Guid OrganizationId,
        DateTime OccurredOnUtc
    ) : IDomainEvent;
}