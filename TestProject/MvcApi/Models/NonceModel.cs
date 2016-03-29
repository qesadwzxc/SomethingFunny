using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApi.Models
{
    public class NonceModel
    {
        private int db_ID;
        /// <summary>
        /// 获取或设置Sys_Nonce表主键ID
        /// </summary>
        public int ID
        {
            get { return db_ID; }
            set { db_ID = value; }
        }

        private string db_Nonce;
        /// <summary>
        /// 获取或设置种子
        /// </summary>
        public string Nonce 
        {
            get { return db_Nonce; }
            set { db_Nonce = value; } 
        }

        private DateTime db_CreateTime;
        /// <summary>
        /// 获取或设置创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return db_CreateTime; }
            set { db_CreateTime = value; }
        }
    }
}