/*
 * Description:interface of the factory that can create ADO.NET object
*/

using System.Data;

namespace Ayx.CSLibrary.ORM.Common
{
    public interface IADOFactory
    {
        string ConnectionString { get; }
        string DbType { get; }
        IDbConnection CreateConnection();
        IDbCommand CreateCommand();
        IDbDataAdapter CreateDataAdapter();
        IDbDataParameter CreateDataParameter(string field, object value);
    }
}
