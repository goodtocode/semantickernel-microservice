namespace Goodtocode.Domain.Types;

public interface IDomainEntity<TModel>
{
    Guid Id { get; }
    string PartitionKey { get; }
    DateTime CreatedOn { get; }
    DateTime? ModifiedOn { get; }
    DateTime? DeletedOn { get; }   
    DateTimeOffset Timestamp { get; }
    void RaiseDomainEvent(IDomainEvent<TModel> domainEvent);
    void ClearDomainEvents();
    bool Equals(object obj);
    int GetHashCode();
}