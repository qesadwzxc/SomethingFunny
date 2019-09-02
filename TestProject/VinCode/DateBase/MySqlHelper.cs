using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;

namespace VinCode.DateBase
{
    public class MySqlHelper : IDisposable
    {
        #region Base
        private bool Disposed;
        private string connString = ConfigurationManager.AppSettings["MySqlConnectionString"].ToString();
        private MySqlConnection conn;
        private string _errorLog;
        public string Errorlog
        {
            get { return this._errorLog; }
            set { this._errorLog = value; }
        }
        public MySqlHelper()
        {
            try
            {
                conn = new MySqlConnection(connString);
                conn.Open();
            }
            catch (Exception ex)
            {
                this.Errorlog = ex.Message;
            }
        }

        ~MySqlHelper()
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
                using (MySqlCommand command = new MySqlCommand(commandString, conn))
                {
                    command.ExecuteNonQuery();
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                this.Errorlog = ex.Message;
                throw;
            }
            return isSuccess;
        }

        public bool Execute(string commandString, List<MySqlParameter> param)
        {
            this.Errorlog = string.Empty;
            MySqlCommand command = new MySqlCommand() { CommandText = commandString, Connection = this.conn };
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
                throw;
            }
            return isSuccess;
        }

        public int ExecuteNonQuery(string commandString)
        {
            this.Errorlog = string.Empty;
            int query = 0;
            try
            {
                using (MySqlCommand command = new MySqlCommand(commandString, conn))
                {
                    query = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                this.Errorlog = ex.Message;
                throw;
            }
            return query;
        }

        public int ExecuteNonQuery(string commandString, List<MySqlParameter> param)
        {
            this.Errorlog = string.Empty;
            MySqlCommand command = new MySqlCommand() { CommandText = commandString, Connection = this.conn };
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
                throw;
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
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(commandString, conn))
                {
                    adapter.Fill(dataTable);
                    dt = dataTable;
                }
            }
            catch (Exception ex)
            {
                this.Errorlog = ex.Message;
                throw;
            }
            return dt;
        }

        public DataTable ExecuteReader(string commandString, List<MySqlParameter> param)
        {
            this.Errorlog = string.Empty;
            MySqlCommand command = new MySqlCommand() { CommandText = commandString, Connection = this.conn };
            DataTable dataTable = new DataTable();
            DataTable dt = null;
            foreach (var p in param)
            {
                command.Parameters.Add(p);
            }

            try
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    adapter.Fill(dataTable);
                    dt = dataTable;
                }
            }
            catch (Exception ex)
            {
                this.Errorlog = ex.Message;
                throw;
            }
            return dt;
        }

        public int ExcuteBulkCopy(DataTable dt)
        {
            try
            {
                if (string.IsNullOrEmpty(dt.TableName)) throw new Exception("请给DataTable的TableName属性附上表名称");
                if (dt.Rows.Count == 0) return 0;
                string tmpPath = Path.GetTempFileName();
                string csv = DataTableToCsv(dt);
                File.WriteAllText(tmpPath, csv);

                MySqlBulkLoader bulk = new MySqlBulkLoader(conn)
                {
                    FieldTerminator = ",",
                    FieldQuotationCharacter = '"',
                    EscapeCharacter = '"',
                    LineTerminator = @"\n",
                    FileName = tmpPath,
                    NumberOfLinesToSkip = 1,
                    TableName = dt.TableName
                };
                return bulk.Load();
            }
            catch (Exception ex)
            {
                this.Errorlog = ex.Message;
                throw;
            }
        }

        private static string DataTableToCsv(DataTable table)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。
            StringBuilder sb = new StringBuilder();
            DataColumn colum;
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    colum = table.Columns[i];
                    if (i != 0) sb.Append(",");
                    if (colum.DataType == typeof(string) && row[colum].ToString().Contains(","))
                    {
                        sb.Append("\"" + row[colum].ToString().Replace("\"", "\"\"") + "\"");
                    }
                    else sb.Append(row[colum].ToString());
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}