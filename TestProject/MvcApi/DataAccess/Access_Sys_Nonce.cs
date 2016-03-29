﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using iThinking.Helper.Db;
using MvcApi.Models;
using System.Data;

namespace MvcApi.DataAccess
{
    public class Access_Sys_Nonce
    {
        private const string connectString = "Data Source=10.14.41.207;Persist Security Info=True;User ID=sa;Password=NewP@ssW0rd;Initial Catalog=testdb;";
        SQLHelper sql = new SQLHelper(connectString);

        /// <summary>
        /// 获取全部种子
        /// </summary>
        /// <returns></returns>
        public DataTable AllNonce()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.AppendFormat(@"SELECT [ID]
                                          ,[Nonce]
                                          ,[CreateTime]
                                      FROM [testdb].[dbo].[Sys_Nonce]");
            return sql.Query(sqlCommand.ToString());
        }

        /// <summary>
        /// 根据主键获取种子
        /// </summary>
        /// <param name="num">主键ID</param>
        /// <returns>种子值</returns>
        public string SelectNonce(int num)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.AppendFormat(@"SELECT [ID]
                                          ,[Nonce]
                                          ,[CreateTime]
                                      FROM [testdb].[dbo].[Sys_Nonce]
                                      WHERE ID={0}",num);
            return sql.GetValue(sqlCommand.ToString(), "Nonce");
        }

        /// <summary>
        /// 创建一个种子
        /// </summary>
        /// <param name="nonce">种子数据模型</param>
        /// <returns></returns>
        public bool CreateNonce(NonceModel nonce)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.AppendFormat(@"INSERT INTO [testdb].[dbo].[Sys_Nonce]
                                           ([Nonce]
                                           ,[CreateTime])
                                      VALUES
                                           ('{0}','{1}')",nonce.Nonce,nonce.CreateTime);
            return sql.Execute(sqlCommand.ToString());
        }
    }
}