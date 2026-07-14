using SharedKernel.DomainEvents;

namespace Domain.Aggregates.Session.Events
{
    public sealed record RefreshTokenRotatedDomainEvent(
        Guid SessionId,
        Guid UserId,
        DateTime OccurredOnUtc
    ) : IDomainEvent;
}