using Ayx.CSLibrary.ORM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace Ayx.CSLibrary.ORM
{
    public class OleDbFactory : IADOFactory
    {
        private string _connectionString;
        public string ConnectionString { get { return _connectionString; } }

        private string _dbType;
        public string DbType { get { return _dbType; } }


        public OleDbFactory(string connectionString,string dbType)
        {
            _connectionString = connectionString;
            _dbType = dbType;
        }

        public IDbConnection CreateConnection()
        {
            return new OleDbConnection();
        }

        public IDbCommand CreateCommand()
        {
            return new OleDbCommand();
        }

        public IDbDataAdapter CreateDataAdapter()
        {
            return new OleDbDataAdapter();
        }

        public IDbDataParameter CreateDataParameter(string field, object value)
        {
            return new OleDbParameter(field, value);
        }
    }
}
