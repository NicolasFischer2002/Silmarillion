using SharedKernel.DomainEvents;

namespace Domain.Aggregates.Session.Events
{
    public sealed record SessionCreatedDomainEvent(
        Guid SessionId,
        Guid UserId,
        DateTime OccurredOnUtc
    ) : IDomainEvent;
}