using System.Data;

namespace chessAPI.dataAccess.interfaces;
public interface IRelationalContext<TC>
        where TC : struct
{
    IDbTransaction trn { get; }
    IDBConcurrencyHandler<TC> concurrencyHandler { get; }
    void getTransactionContext();
    void commitTransactionContext();
    void closeTransactionContext();
    void rollbackTransaction(Exception ex);
    void rollbackTransaction(ApplicationException ex);
}