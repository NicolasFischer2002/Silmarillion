using SharedKernel.DomainEvents;

namespace Domain.Aggregates.Role.Events
{
    public sealed record RoleCreatedDomainEvent(
        Guid RoleId,
        Guid OrganizationId,
        DateTime OccurredOnUtc) : IDomainEvent;
}