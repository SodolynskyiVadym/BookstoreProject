using Dapper;

namespace BookstoreAPI.Helpers;

public class DataHelper
{
    private readonly DataContextDapper _dapper;
    
    public DataHelper(DataContextDapper dapper)
    {
        _dapper = dapper;
    }
    
    public T? GetById<T>(string tableName, int? id)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@Id", id, System.Data.DbType.Int64);
        return _dapper.LoadDataSingleWithParameters<T>($"SELECT * FROM book_schema.{tableName} WHERE id=@Id", parameters);
    }

    public IEnumerable<T> GetByParameter<T>(string tableName, string parameterName, object parameterValue)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@Value", parameterValue, System.Data.DbType.Object);
        return _dapper.LoadDataWithParameters<T>($"SELECT * FROM book_schema.{tableName} WHERE {parameterName}=@Value", parameters);
    }
        
}