using SharedKernel.DomainEvents;

namespace Domain.Aggregates.Roles.Events
{
    public sealed record RoleCreatedDomainEvent(
        Guid RoleId,
        Guid OrganizationId,
        DateTime OccurredOnUtc) : IDomainEvent;
}