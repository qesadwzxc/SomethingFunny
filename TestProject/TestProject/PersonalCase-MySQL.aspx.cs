using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace TestProject
{
    public partial class PersonalCase_MySQL : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

        }

        protected void btnClick_Click(object sender, EventArgs e)
        {
            MySQLHelper helper = new MySQLHelper();
            HttpCookie cookie = new HttpCookie("Lee");
            cookie.Value = "Vincent";
            cookie.Domain = "10.14.41.197";
            cookie.Expires = DateTime.Now.AddDays(1);

            string command = "select * from dbtest.student limit 50";
            try
            {
                lblResult.Text = helper.Execute(command).ToString();
            }
            catch
            {
                lblResult.Text = helper.Errorlog;
            }
        }

        protected void btnClick_Click1(object sender, EventArgs e)
        {
            MySQLHelper helper = new MySQLHelper();

            string command = "select * from dbtest.student limit ?pa";
            var collect = new List<MySqlParameter>();
            collect.Add(new MySqlParameter("?pa",int.Parse(txtLalala.Text)));
            try
            {
                lblResult.Text = helper.ExecuteNonQuery(command, collect).ToString();
            }
            catch
            {
                lblResult.Text = helper.Errorlog;
            }
        }

        protected void btnClick_Click2(object sender, EventArgs e)
        {
            MySQLHelper helper = new MySQLHelper();

            string command = "select * from dbtest.student limit 50";
            try
            {
                lblResult.Text = helper.ExecuteReader(command).ToString();
                DataTable dt = helper.ExecuteReader(command);
            }
            catch
            {
                lblResult.Text = helper.Errorlog;
            }
        }
    }

    public class MySQLHelper : IDisposable
    {
        #region Base
        private bool Disposed;
        private string connString = "Data Source='10.1.25.57';Port='1231';User Id='Lee';Password='123';charset='utf8';pooling=true;Allow Zero Datetime=True";
        private MySqlConnection conn;
        private string _errorLog;
        public string Errorlog
        {
            get { return this._errorLog; }
            set { this._errorLog = value; }
        }
        public MySQLHelper()
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

        ~MySQLHelper() 
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

        public bool Execute(string commandString,List<MySqlParameter> param)
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
    }
}