/*
 * Description:The base class of data operation
*/

using Ayx.CSLibrary.ORM.Common;
using Ayx.CSLibrary.ORM.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Ayx.CSLibrary.ORM
{
    public class AyxORM
    {
        #region Properties

        public IADOFactory ADOFactory { get; set; }
        public string DbType { get { return ADOFactory.DbType; } }
        public string ConnectionString { get{ return ADOFactory.ConnectionString; } }

        #endregion

        #region Constructure

        public AyxORM(IADOFactory adoFactory)
        {
            ADOFactory = adoFactory;
        }

        #endregion

        #region Factory

        public static AyxORM UseExcel2003(string filename)
        {
            var conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename +
                         ";Extended Properties='Excel 8.0;HDR=False;IMEX=1'";
            return new AyxORM(new OleDbFactory(conStr, "Excel"));
        }

        public static AyxORM UseAccess2003(string fileName, string password = null)
        {
            var conStr = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                         "Data Source=" + fileName;
            if (!string.IsNullOrEmpty(password))
                conStr += "Jet OLEDB:Database Password=" + password;
            return new AyxORM(new OleDbFactory(conStr, "Access"));
        }

        #endregion

        #region Base data methods

        public int ExecuteNonQuery(string sql, object param = null, IDbTransaction transaction = null)
        {
            var cmd = GetCommand(sql, param, transaction);
            return ExecuteNonQuery(cmd);
        }

        //public int ExecuteNonQuery(string sql, DbParams param = null, IDbTransaction transaction = null)
        //{
        //    var cmd = GetCommand(sql, param, transaction);
        //    return ExecuteNonQuery(cmd);
        //}

        public int ExecuteNonQuery(IDbCommand cmd)
        {
            try
            {
                if (cmd.Transaction == null)
                {
                    cmd.Connection.Open();
                    var count = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return count;
                }
                else
                {
                    return cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                cmd.Connection.Close();
                throw;
            }
        }

        public DataSet ExecuteDataSet(string sql, object param = null, IDbTransaction transaction = null)
        {
            var cmd = GetCommand(sql, param, transaction);
            return ExecuteDataSet(cmd);
        }

        //public DataSet ExecuteDataSet(string sql, DbParams param = null, IDbTransaction transaction = null)
        //{
        //    var cmd = GetCommand(sql, param, transaction);
        //    return ExecuteDataSet(cmd);
        //}

        public DataSet ExecuteDataSet(IDbCommand cmd)
        {
            var da = GetAdapter(cmd);
            var ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public DataTable ExecuteDataTable(string sql, object param = null, IDbTransaction transaction = null)
        {
            return ExecuteDataSet(sql, param, transaction).Tables[0];
        }

        //public DataTable ExecuteDataTable(string sql, DbParams param = null, IDbTransaction transaction = null)
        //{

        //    return ExecuteDataSet(sql, param, transaction).Tables[0];
        //}

        public DataTable ExecuteDataTable(IDbCommand cmd)
        {
            return ExecuteDataSet(cmd).Tables[0];
        }

        #endregion

        #region ORM data methods

        public void CreateTableIfNotExist(string sql)
        {
            if (string.IsNullOrEmpty(sql))
                throw new AyxORMException("no SQL to create table");
            ExecuteNonQuery(sql);
        }

        public IEnumerable<T> ExecuteList<T>(string sql, object param = null , IDbTransaction transaction = null)
        {
            var dt = ExecuteDataTable(sql, param, transaction);
            return new Mapper<T>().From(dt);
        }

        public IEnumerable<T> Select<T>(string where = "", object param= null, IDbTransaction transaction = null)
        {
            var sql = SQLGenerator.GetSelectSQL<T>(where);
            var cmd = GetCommand(sql, param, transaction);
            try
            {
                if (transaction == null)
                    cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                var result = new Mapper<T>().From(reader).ToList();
                reader.Close();
                if (transaction == null)
                    cmd.Connection.Close();
                return result;
            }
            catch
            {
                if(transaction == null)
                    cmd.Connection.Close();
                throw;
            }
        }

        public int Delete<T>(T item, IDbTransaction transaction = null)
        {
            var sql = SQLGenerator.GetDeleteSQL<T>();
            var cmd = GetCommand(sql, null, transaction);
            var keyProperty = DbAttributes.GetPrimaryKeyProperty<T>();
            AddDataParameter(cmd, "@" + keyProperty.Name, keyProperty.GetValue(item, null));
            return ExecuteNonQuery(cmd);
        }

        public int Delete<T>(string where, object param = null, IDbTransaction transaction = null)
        {
            var sql = SQLGenerator.GetDeleteSQL<T>(where);
            var cmd = GetCommand(sql, param, transaction);
            return ExecuteNonQuery(cmd);
        }

        public int Update<T>(T item, IDbTransaction transaction = null)
        {
            var mapping = FieldMapping.GetUpdateMapping<T>();
            var sql = SQLGenerator.GetUpdateSQL<T>(mapping);
            var cmd = GetCommand(sql, item, transaction);
            return ExecuteNonQuery(cmd);
        }

        public int Update<T>(IList<string> fields, T item, IDbTransaction transaction = null)
        {
            if (fields == null || !fields.Any())
                return Update<T>(item, transaction);
            var sql = SQLGenerator.GetUpdateSQL<T>(fields);
            var cmd = GetCommand(sql, item,transaction);
            return ExecuteNonQuery(cmd);
        }

        public int Insert<T>(T item, IDbTransaction transaction = null)
        {
            var mapping = FieldMapping.GetInsertMapping<T>();
            var sql = SQLGenerator.GetInsertSQL<T>(mapping);
            var cmd = GetCommand(sql, null, transaction);
            AddDataParameters(cmd, item, mapping);
            return ExecuteNonQuery(cmd);
        }

        public int InsertAndGetID<T>(T item, IDbTransaction transaction = null)
        {
            var newTrans = false;
            if(transaction == null)
            {
                transaction = GetTransaction();
                newTrans = true;
            }
            try
            {
                Insert<T>(item, transaction);
                var cmd = GetCommand(SQLGenerator.GetIdentitySQL(), null, transaction);
                var result = cmd.ExecuteScalar();
                if(newTrans)
                    transaction.Commit();
                return (int)result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public int InsertList<T>(IList<T> items, IDbTransaction transaction = null)
        {
            bool isNewTransaction = false;
            var mapping = FieldMapping.GetInsertMapping<T>();
            var sql = SQLGenerator.GetInsertSQL<T>(mapping);
            if (transaction == null)
            {
                transaction = GetTransaction();
                isNewTransaction = true;
            }
            var cmd = GetCommand(sql, null, transaction);

            try
            {
                foreach (var item in items)
                {
                    cmd.Parameters.Clear();
                    AddDataParameters(cmd, item, mapping);
                    ExecuteNonQuery(cmd);
                }
                if (isNewTransaction)
                {
                    transaction.Commit();
                }
            }
            catch(Exception e)
            {
                transaction.Rollback();
                transaction.Connection.Close();
                throw;
            }
            return items.Count;
        }

        public int ClearTable<T>(IDbTransaction transaction = null)
        {
            var sql = SQLGenerator.GetClearSQL<T>();
            return ExecuteNonQuery(sql, null, transaction);
        }

        #endregion

        #region TransactionMethods

        public IDbTransaction GetTransaction()
        {
            var con = GetConnection();
            con.Open();
            return con.BeginTransaction();
        }

        #endregion

        #region Public methods

        public void AddDataParameters(IDbCommand cmd, object param)
        {
            if (param == null) return;
            var t = param.GetType();
            foreach (var property in t.GetProperties())
            {
                if (!DbAttributes.IsDbField(property)) continue;
                AddDataParameter(cmd, param, property);
            }
        }

        public void AddDataParameters<T>(IDbCommand cmd, T item, Dictionary<PropertyInfo,string> mapping)
        {
            foreach (var map in mapping)
            {
                AddDataParameter(cmd, item, map.Key);
            }
        }

        //private void AddDataParameters(IDbCommand cmd, DbParams paramDict)
        //{
        //    if (paramDict == null) return;
        //    foreach (var param in paramDict)
        //    {
        //        AddDataParameter(cmd, param.Key, param.Value);
        //    }
        //}

        public void AddDataParameter(IDbCommand cmd, string key, object value)
        {
            cmd.Parameters.Add(ADOFactory.CreateDataParameter(key, value));
        }

        public void AddDataParameter<T>(IDbCommand cmd, T item, PropertyInfo property)
        {
            AddDataParameter(cmd, "@" + property.Name, property.GetValue(item, null));
        }
        public IDbConnection GetConnection(IDbTransaction transaction)
        {
            if (transaction != null)
                return transaction.Connection;
            else
                return GetConnection();
        }

        public IDbConnection GetConnection()
        {
            var result = ADOFactory.CreateConnection();
            result.ConnectionString = ConnectionString;
            return result;
        }

        public IDbCommand GetCommand(string sql,object param, IDbTransaction transaction)
        {
            var con = GetConnection(transaction);
            var cmd = ADOFactory.CreateCommand();
            cmd.CommandText = sql;
            cmd.Connection = con;
            cmd.Transaction = transaction;
            AddDataParameters(cmd, param);
            return cmd;
        }

        //private IDbCommand GetCommand(string sql, DbParams paramList, IDbTransaction transaction)
        //{
        //    var con = GetConnection(transaction);
        //    var cmd = ADOFactory.CreateCommand();
        //    cmd.CommandText = sql;
        //    cmd.Connection = con;
        //    cmd.Transaction = transaction;
        //    AddDataParameters(cmd, paramList);
        //    return cmd;
        //}

        public IDataAdapter GetAdapter(IDbCommand cmd)
        {
            var result = ADOFactory.CreateDataAdapter();
            result.SelectCommand = cmd;
            return result;
        }
        #endregion
    }
}
