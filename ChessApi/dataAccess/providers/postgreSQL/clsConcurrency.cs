using chessAPI.dataAccess.interfaces;
using Dapper;

namespace chessAPI.dataAccess.providers.postgreSQL;

public sealed class clsConcurrency<TC> : IDBConcurrencyHandler<TC>
        where TC : struct
{
    private const string columnName = "rv";

    public TC getConcurrencyStamp(ref DynamicParameters parameters)
    {
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));
        if (parameters != null)
        {
            return parameters.Get<TC>(columnName);
        }
        throw new ArgumentNullException(nameof(parameters));
    }

    public void optimisticConcurrencyColumnAsParam(TC rowVersion, ref DynamicParameters parameters)
    {
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));
        parameters.Add(columnName, rowVersion);
    }
}