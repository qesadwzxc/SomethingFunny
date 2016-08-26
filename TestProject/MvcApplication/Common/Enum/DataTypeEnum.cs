using System.ComponentModel;

namespace MvcApplication.Common
{
    /// <summary>
    /// DataColumn.DataType
    /// </summary>
    public enum DataTypeEnum
    {
        [Description("System.String")]
        String = 0,
        [Description("System.Int32")]
        Int = 1,
        [Description("System.Decimal")]
        Decimal = 2,
        [Description("System.DateTime")]
        DateTime = 3,
        [Description("System.Boolean")]
        Boolean = 4,
        [Description("System.TimeSpan")]
        TimeSpan = 5,
        [Description("")]
        Empty = 6
    }
}