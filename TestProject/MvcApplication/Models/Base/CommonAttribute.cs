using System;

namespace MvcApplication.Models
{
    /// <summary>
    /// 实体类对应的表名
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = true)]
    public class TableAttribute : System.Attribute
    {
        public TableAttribute(string tableName)
        {
            this.tableName = tableName;
        }

        private string tableName;
        /// <summary>
        /// 实体实际对应的表名
        /// </summary>
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }
    }

    /// <summary>
    /// 标记属于表的实体类字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = true)]
    public class PropertyAttribute : System.Attribute
    {

    }

    /// <summary>
    /// 标记属于表的有效字段名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = true)]
    public class ValidAttribute : System.Attribute
    {

    }

    /// <summary>
    /// 标记自增长字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = true)]
    public class IncreaseAttribute : System.Attribute
    {

    }

    /// <summary>
    /// 标记主键字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = true)]
    public class KeyPropertyAttribute : System.Attribute
    {

    }

    /// <summary>
    /// 标记创建信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = true)]
    public class CreateAttribute : System.Attribute
    {

    }

    /// <summary>
    /// 标记修改信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = true)]
    public class ModifyAttribute : System.Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class JoinAttribute : System.Attribute
    {

        private string _onstring = "";
        private string[] _selectstring;

        public string OnString
        {
            get
            {
                return _onstring;
            }
        }

        public string[] SelectString
        {
            get
            {
                return _selectstring;
            }
        }
        private JoinOperator _jointype;
        public JoinOperator JoinType
        {
            get
            {
                return _jointype;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="onstring">on的内容</param>
        /// <param name="selectstring">查询的列</param>
        public JoinAttribute(JoinOperator joinType, string onstring, params string[] selectstring)
        {
            _onstring = onstring;
            _selectstring = selectstring;
            _jointype = joinType;
        }
    }

    public enum JoinOperator
    {
        LeftJoin,
        Join
    }
}