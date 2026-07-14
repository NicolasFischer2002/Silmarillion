using SharedKernel.DomainEvents;

namespace Domain.Aggregates.Session.Events
{
    public sealed record SessionRevokedDomainEvent(
        Guid SessionId,
        Guid UserId,
        DateTime OccurredOnUtc
    ) : IDomainEvent;
}