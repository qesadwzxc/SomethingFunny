using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MvcApplication.Models.Base
{
    public sealed class SqlParameterWrapper
    {
        private SqlParameter param;

        public SqlParameter Parameter
        {
            get
            {
                return param;
            }
        }

        public SqlParameterWrapper(SqlParameter item)
        {
            if (item.ParameterName != null && !(item.ParameterName.Trim() == string.Empty))
            {
                throw new ArgumentNullException("");
            }
            if (item.Value == null)
            {
                throw new ArgumentNullException();
            }
            param = item;
        }

        public SqlParameterWrapper(string paramName, object paramValue)
        {
            param = new SqlParameter(paramName, paramValue);
        }

        public SqlParameterWrapper(string paramName, object paramValue, ParameterDirection paramDirection)
        {
            param = new SqlParameter(paramName, paramValue);
            param.Direction = paramDirection;
        }

        public SqlParameterWrapper(string paramName, object paramValue, SqlDbType paramType)
        {
            param = new SqlParameter(paramName, paramType);
            param.Value = paramValue;
        }

        public SqlParameterWrapper(string paramName, object paramValue, SqlDbType paramType, ParameterDirection paramDirection)
        {
            param = new SqlParameter(paramName, paramValue);
            param.DbType = (DbType)paramType;
            param.Direction = paramDirection;
        }

        static SqlParameterWrapper()
        {
        }
    }

    public sealed class SqlParameterWrapperCollection : IEnumerable, ICollection<SqlParameterWrapper>, IList<SqlParameterWrapper>, IEnumerable<SqlParameterWrapper>
    {
        private IList<SqlParameterWrapper> paramList = new List<SqlParameterWrapper>();

        public int Count
        {
            get
            {
                return paramList.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return paramList.IsReadOnly;
            }
        }

        public SqlParameterWrapper this[int index]
        {
            get
            {
                return paramList[index];
            }
            set
            {
                paramList[index] = value;
            }
        }

        public void Add(SqlParameterWrapper item)
        {
            paramList.Add(item);
        }

        public void Clear()
        {
            paramList.Clear();
        }

        public bool Contains(SqlParameterWrapper item)
        {
            return paramList.Contains(item);
        }

        public SqlParameter[] ConvertToSqlParameter()
        {
            SqlParameter[] array = new SqlParameter[paramList.Count];
            for (int i = 0; i < paramList.Count; i++)
            {
                array[i] = paramList[i].Parameter;
            }
            return array;
        }

        public void CopyTo(SqlParameterWrapper[] array, int arrayIndex)
        {
            paramList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<SqlParameterWrapper> GetEnumerator()
        {
            return paramList.GetEnumerator();
        }

        public int IndexOf(SqlParameterWrapper item)
        {
            return paramList.IndexOf(item);
        }

        public void Insert(int index, SqlParameterWrapper item)
        {
            paramList.Insert(index, item);
        }

        public bool Remove(SqlParameterWrapper item)
        {
            return paramList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            paramList.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return paramList.GetEnumerator();
        }

        static SqlParameterWrapperCollection()
        {
        }
    }
}