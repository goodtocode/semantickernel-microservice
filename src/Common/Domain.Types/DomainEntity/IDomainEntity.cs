namespace Goodtocode.Domain.Types;

public interface IDomainEntity<TModel>
{
    Guid Key { get; }
    string PartitionKey { get; }
    void RaiseDomainEvent(IDomainEvent<TModel> domainEvent);
    void ClearDomainEvents();
    bool Equals(object obj);
    int GetHashCode();
}