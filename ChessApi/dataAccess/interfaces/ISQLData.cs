namespace chessAPI.dataAccess.interfaces;

public interface ISQLData
    {
        string SQLGetAll { get; }
        string SQLDataEntity { get; }
        string NewDataEntity { get; }
        string DeleteDataEntity { get; }
        string UpdateWholeEntity { get; }
    }