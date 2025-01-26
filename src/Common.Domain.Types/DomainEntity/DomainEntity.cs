using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Goodtocode.Domain.Types;

public abstract class DomainEntity<TModel> : IDomainEntity<TModel>
{
    private readonly List<IDomainEvent<TModel>> _domainEvents = [];

    [Key]
    public Guid Id { get; set; }
    public string PartitionKey { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedOn { get; set; }
    public DateTime? DeletedOn { get; set; }    
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

    [IgnoreDataMember]
    public IReadOnlyList<IDomainEvent<TModel>> DomainEvents => _domainEvents;

    protected DomainEntity()
    {
    }

    protected DomainEntity(Guid id)
        : this()
    {
        this.Id = id;
    }    

    public void AddDomainEvent(IDomainEvent<TModel> domainEvent)
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

        if (Id == Guid.Empty || other.Id == Guid.Empty)
            return false;

        return Id == other.Id;
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
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + GetRealType().ToString().GetHashCode(StringComparison.Ordinal);
            hash = hash * 23 + Id.GetHashCode();
            return hash;
        }
    }


    private Type GetRealType(string namespaceRoot = "")
    {
        var type = GetType();

        if (type.ToString().Contains(namespaceRoot, StringComparison.InvariantCulture))
            return type.BaseType ?? type.GetType();

        return type;
    }
}