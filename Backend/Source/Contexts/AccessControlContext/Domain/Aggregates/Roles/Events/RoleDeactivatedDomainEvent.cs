using SharedKernel.DomainEvents;

namespace Domain.Aggregates.Roles.Events
{
    public sealed record RoleDeactivatedDomainEvent(
        Guid RoleId,
        Guid OrganizationId,
        DateTime OccurredOnUtc) : IDomainEvent;
}