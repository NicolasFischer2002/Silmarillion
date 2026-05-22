namespace SharedKernel.DomainEvents
{
    public interface IDomainEvent
    {
        DateTime OccurredOnUtc { get; }
    }
}