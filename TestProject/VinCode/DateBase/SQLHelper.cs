using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Common;

namespace VinCode.DateBase
{
    public class SqlHelper : IDisposable
    {
        #region Base
        private bool Disposed;
        private string connString = ConfigurationManager.AppSettings["SqlConnectionString"].ToString();
        private SqlConnection conn;
        private string _errorLog;
        public string Errorlog
        {
            get { return _errorLog; }
            set { _errorLog = value; }
        }
        public SqlHelper()
        {
            if (string.IsNullOrWhiteSpace(connString))
            {
                throw new Exception("获取sql连接字符串失败");
            }
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();
            }
            catch (Exception ex)
            {
                Errorlog = ex.Message;
                throw;
            }
        }

        ~SqlHelper()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!Disposed && disposing)
            {
                try
                {
                    conn.Close();
                    conn.Dispose();
                }
                catch
                { }
            }
            Disposed = true;
        }
        #endregion
        /// <summary>
        /// 执行语句返回查询结果第一行第一列
        /// </summary>
        /// <param name="commandString">sql语句</param>
        /// <param name="trans">事务</param>
        /// <returns></returns>
        public object ExecuteScalar(string commandString, SqlTransaction trans = null)
        {
            return ExecuteScalar(commandString, new List<SqlParameter>(), trans);
        }

        /// <summary>
        /// 执行语句返回查询结果第一行第一列
        /// </summary>
        /// <param name="commandString">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="trans">事务</param>
        /// <returns></returns>
        public object ExecuteScalar(string commandString, SqlParameter param, SqlTransaction trans = null)
        {
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(param);
            return ExecuteScalar(commandString, paramList, trans);
        }

        /// <summary>
        /// 执行语句返回查询结果第一行第一列
        /// </summary>
        /// <param name="commandString">sql语句</param>
        /// <param name="paramList">参数</param>
        /// <param name="trans">事务</param>
        /// <returns></returns>
        public object ExecuteScalar(string commandString, List<SqlParameter> paramList, SqlTransaction trans = null)
        {
            Errorlog = string.Empty;
            SqlCommand command = GetCommand(commandString, paramList, trans);
            object item;

            try
            {
                item = command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Errorlog = ex.Message;
                throw;
            }
            return item;
        }

        /// <summary>
        /// 执行语句返回查询结果第一行第一列(数值类型)
        /// </summary>
        /// <param name="commandString">sql语句</param>
        /// <param name="trans">事务</param>
        /// <returns></returns>
        public int ExecuteScalarByInt(string commandString, SqlTransaction trans = null)
        {
            return ExecuteScalarByInt(commandString, new List<SqlParameter>(), trans);
        }

        /// <summary>
        /// 执行语句返回查询结果第一行第一列(数值类型)
        /// </summary>
        /// <param name="commandString">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="trans">事务</param>
        /// <returns></returns>
        public object ExecuteScalarByInt(string commandString, SqlParameter param, SqlTransaction trans = null)
        {
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(param);
            return ExecuteScalarByInt(commandString, paramList, trans);
        }

        /// <summary>
        /// 执行语句返回查询结果第一行第一列(数值类型)
        /// </summary>
        /// <param name="commandString">sql语句</param>
        /// <param name="paramList">参数</param>
        /// <param name="trans">事务</param>
        /// <returns></returns>
        public int ExecuteScalarByInt(string commandString, List<SqlParameter> paramList, SqlTransaction trans = null)
        {
            object item = ExecuteScalar(commandString, paramList, trans);
            int itemInt = 0;
            int.TryParse(item.ToString(), out itemInt);
            return itemInt;
        }

        /// <summary>
        /// 执行语句返回受影响的行数
        /// </summary>
        /// <param name="commandString">sql语句</param>
        /// <param name="trans">事务</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandString, SqlTransaction trans = null)
        {
            return ExecuteNonQuery(commandString, new List<SqlParameter>(), trans);
        }

        /// <summary>
        /// 执行语句返回受影响的行数
        /// </summary>
        /// <param name="commandString">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="trans">事务</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandString, SqlParameter param, SqlTransaction trans = null)
        {
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(param);
            return ExecuteNonQuery(commandString, paramList, trans);
        }

        /// <summary>
        /// 执行语句返回受影响的行数
        /// </summary>
        /// <param name="commandString">sql语句</param>
        /// <param name="paramList">参数</param>
        /// <param name="trans">事务</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandString, List<SqlParameter> paramList, SqlTransaction trans = null)
        {
            Errorlog = string.Empty;
            SqlCommand command = GetCommand(commandString, paramList, trans);
            int query = 0;

            try
            {
                query = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Errorlog = ex.Message;
                throw;
            }
            return query;
        }

        /// <summary>
        /// 执行语句返回查询到的表格
        /// </summary>
        /// <param name="commandString">sql语句</param>
        /// <param name="trans">事务</param>
        /// <returns></returns>
        public DataTable ExecuteReader(string commandString, SqlTransaction trans = null)
        {
            return ExecuteReader(commandString, new List<SqlParameter>(), trans);
        }

        /// <summary>
        /// 执行语句返回查询到的表格
        /// </summary>
        /// <param name="commandString">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="trans">事务</param>
        /// <returns></returns>
        public DataTable ExecuteReader(string commandString, SqlParameter param, SqlTransaction trans = null)
        {
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(param);
            return ExecuteReader(commandString, paramList, trans);
        }

        /// <summary>
        /// 执行语句返回查询到的表格
        /// </summary>
        /// <param name="commandString">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="trans">事务</param>
        /// <returns></returns>
        public DataTable ExecuteReader(string commandString, List<SqlParameter> paramList, SqlTransaction trans = null)
        {
            Errorlog = string.Empty;
            SqlCommand command = GetCommand(commandString, paramList, trans);
            DataTable dataTable = new DataTable();
            DataTable dt = null;

            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dataTable);
                    dt = dataTable;
                }
            }
            catch (Exception ex)
            {
                Errorlog = ex.Message;
                throw;
            }
            return dt;
        }

        /// <summary>
        /// 执行语句并返回主键（多条记录返回最后一条的主键）
        /// </summary>
        /// <param name="commandString">sql语句</param>
        /// <param name="trans">事务</param>
        /// <returns></returns>
        public int ExecuteIdentity(string commandString, SqlTransaction trans = null)
        {
            return ExecuteIdentity(commandString, new List<SqlParameter>(), trans);
        }

        /// <summary>
        /// 执行语句并返回主键（多条记录返回最后一条的主键）
        /// </summary>
        /// <param name="commandString">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="trans">事务</param>
        /// <returns></returns>
        public int ExecuteIdentity(string commandString, SqlParameter param, SqlTransaction trans = null)
        {
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(param);
            return ExecuteIdentity(commandString, paramList, trans);
        }

        /// <summary>
        /// 执行语句并返回主键（多条记录返回最后一条的主键）
        /// </summary>
        /// <param name="commandString">sql语句</param>
        /// <param name="paramList">参数</param>
        /// <param name="trans">事务</param>
        /// <returns></returns>
        public int ExecuteIdentity(string commandString, List<SqlParameter> paramList, SqlTransaction trans = null)
        {
            commandString += " SELECT SCOPE_IDENTITY()";
            int itemInt = ExecuteScalarByInt(commandString, paramList, trans);
            return itemInt;
        }

        /// <summary>
        /// 获取SQLCommand
        /// </summary>
        /// <param name="commandString">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="trans">事务</param>
        /// <returns></returns>
        private SqlCommand GetCommand(string commandString, List<SqlParameter> param, SqlTransaction trans)
        {
            SqlCommand command = new SqlCommand() { CommandText = commandString, Connection = conn, Transaction = trans };
            if (param != null && param.Count > 0)
            {
                foreach (var p in param)
                {
                    command.Parameters.Add(p);
                }
            }
            return command;
        }
    }
}