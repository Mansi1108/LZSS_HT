using Dapper;

namespace chessAPI.dataAccess.interfaces;

public interface IDBConcurrencyHandler<TC>
        where TC : struct
    {
        void optimisticConcurrencyColumnAsParam(TC rowVersion, ref DynamicParameters parameters);
        TC getConcurrencyStamp(ref DynamicParameters parameters);
    }