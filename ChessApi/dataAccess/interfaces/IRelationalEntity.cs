namespace chessAPI.dataAccess.interfaces;

public interface IRelationalEntity<TKey, TConcurrency>
        where TConcurrency : struct
{
    TKey key { get; set; }
    TConcurrency? rv { get; set; }
}