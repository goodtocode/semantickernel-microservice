namespace Goodtocode.Domain.Types;

public interface IDomainEvent<T>
{
    T Item { get; }
}