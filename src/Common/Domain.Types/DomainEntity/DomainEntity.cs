using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Goodtocode.Domain.Types;

public abstract class DomainEntity<TModel> : IDomainEntity<TModel>
{
    private readonly List<IDomainEvent<TModel>> _domainEvents = [];

    [Key]
    public Guid Key { get; }

    [IgnoreDataMember]
    public string PartitionKey { get; set; } = string.Empty;
    
    [IgnoreDataMember]
    public IReadOnlyList<IDomainEvent<TModel>> DomainEvents => _domainEvents;

    protected DomainEntity()
    {
    }

    protected DomainEntity(Guid key)
        : this()
    {
        Key = key;
    }    

    public void RaiseDomainEvent(IDomainEvent<TModel> domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not DomainEntity<TModel> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetRealType() != other.GetRealType())
            return false;

        if (Key == Guid.Empty || other.Key == Guid.Empty)
            return false;

        return Key == other.Key;
    }

    public static bool operator ==(DomainEntity<TModel>? a, DomainEntity<TModel>? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(DomainEntity<TModel>? a, DomainEntity<TModel>? b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetRealType().ToString() + Key).GetHashCode();
    }

    private Type GetRealType(string namespaceRoot = "")
    {
        var type = GetType();

        if (type.ToString().Contains(namespaceRoot))
            return type.BaseType ?? type.GetType();

        return type;
    }
}