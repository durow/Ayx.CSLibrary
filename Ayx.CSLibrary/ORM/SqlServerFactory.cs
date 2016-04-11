/*
 * Description:SqlServer ADO object factory
*/

using Ayx.CSLibrary.ORM.Common;
using System.Data;
using System.Data.SqlClient;

namespace Ayx.CSLibrary.ORM
{
    public class SqlServerFactory : IADOFactory
    {
        #region Properties

        private string _connectionString;
        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }
        public string DbType { get { return "SqlServer"; } }

        #endregion

        #region Constructure
        public SqlServerFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlServerFactory(string ip, string username, string userpass, string dataName)
        {
            _connectionString = "Data Source=" + ip +
                                          ";Initial Catalog=" + dataName +
                                          ";User ID=" + username +
                                          ";Password=" + userpass;
        }

        #endregion

        #region Implented interface methods

        //创建SqlServer链接
        public IDbConnection CreateConnection()
        {
            return new SqlConnection();
        }

        //创建SqlServer命令
        public IDbCommand CreateCommand()
        {
            return new SqlCommand();
        }

        //创建SqlServer DataAdapter
        public IDbDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter();
        }

        //创建SqlServer查询参数
        public IDbDataParameter CreateDataParameter(string field, object value)
        {
            return new SqlParameter(field, value);
        }

        #endregion
    }
}
