using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

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
            get { return this._errorLog; }
            set { this._errorLog = value; }
        }
        public SqlHelper()
        {
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();
            }
            catch (Exception ex)
            {
                this.Errorlog = ex.Message;
            }
        }

        ~SqlHelper()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.Disposed && disposing)
            {
                try
                {
                    conn.Close();
                    conn.Dispose();
                }
                catch
                { }
            }
            this.Disposed = true;
        }
        #endregion

        public bool Execute(string commandString)
        {
            this.Errorlog = string.Empty;
            bool isSuccess = false;
            try
            {
                using (SqlCommand command = new SqlCommand(commandString, conn))
                {
                    command.ExecuteNonQuery();
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                this.Errorlog = ex.Message;
                throw ex;
            }
            return isSuccess;
        }

        public bool Execute(string commandString, List<SqlParameter> param)
        {
            this.Errorlog = string.Empty;
            SqlCommand command = new SqlCommand() { CommandText = commandString, Connection = this.conn };
            bool isSuccess = false;
            foreach (var p in param)
            {
                command.Parameters.Add(p);
            }

            try
            {
                command.ExecuteNonQuery();
                isSuccess = true;
            }
            catch (Exception ex)
            {
                this.Errorlog = ex.Message;
                throw ex;
            }
            return isSuccess;
        }

        public int ExecuteNonQuery(string commandString)
        {
            this.Errorlog = string.Empty;
            int query = 0;
            try
            {
                using (SqlCommand command = new SqlCommand(commandString, conn))
                {
                    query = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                this.Errorlog = ex.Message;
                throw ex;
            }
            return query;
        }

        public int ExecuteNonQuery(string commandString, List<SqlParameter> param)
        {
            this.Errorlog = string.Empty;
            SqlCommand command = new SqlCommand() { CommandText = commandString, Connection = this.conn };
            int query = 0;
            foreach (var p in param)
            {
                command.Parameters.Add(p);
            }

            try
            {
                query = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                this.Errorlog = ex.Message;
                throw ex;
            }
            return query;
        }

        public DataTable ExecuteReader(string commandString)
        {
            this.Errorlog = string.Empty;
            DataTable dataTable = new DataTable();
            DataTable dt = null;

            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(commandString, conn))
                {
                    adapter.Fill(dataTable);
                    dt = dataTable;
                }
            }
            catch (Exception ex)
            {
                this.Errorlog = ex.Message;
                throw ex;
            }
            return dt;
        }

        public DataTable ExecuteReader(string commandString, List<SqlParameter> param)
        {
            this.Errorlog = string.Empty;
            SqlCommand command = new SqlCommand() { CommandText = commandString, Connection = this.conn };
            DataTable dataTable = new DataTable();
            DataTable dt = null;
            foreach (var p in param)
            {
                command.Parameters.Add(p);
            }

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
                this.Errorlog = ex.Message;
                throw ex;
            }
            return dt;
        }
    }
}