using chessAPI.dataAccess.interfaces;

namespace chessAPI.dataAccess.common;

public abstract class relationalEntity<TKey, TConcurrency> : IRelationalEntity<TKey, TConcurrency>, IEquatable<relationalEntity<TKey, TConcurrency>>
        where TKey : IEquatable<TKey>
        where TConcurrency : struct
{
    public abstract TKey key { get; set; }
    public virtual TConcurrency? rv { get; set; }

    public bool Equals(relationalEntity<TKey, TConcurrency>? other) => other != null && key.Equals(other.key);

    public override bool Equals(object? obj) => Equals(obj as relationalEntity<TKey, TConcurrency>);

    public override int GetHashCode() => base.GetHashCode();
}