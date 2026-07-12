using SharedKernel.DomainEvents;

namespace Domain.Aggregates.Membership.Events
{
    public sealed record MembershipActivatedDomainEvent(
        Guid MembershipId,
        Guid UserId,
        Guid OrganizationId,
        DateTime OccurredOnUtc
    ) : IDomainEvent;
}