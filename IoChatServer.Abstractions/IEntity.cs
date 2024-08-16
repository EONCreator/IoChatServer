namespace IoChatServer.Abstractions;

public interface IEntity<TKey>
{
    public TKey Id { get; }
}