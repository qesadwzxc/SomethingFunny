using System.ComponentModel;

namespace MvcApplication.Common
{
    public enum PageStatus
    {
        [Description("新增状态")]
        Add = 0,
        [Description("编辑状态")]
        Edit = 1
    }

    public enum RecordStatus
    {
        [Description("新增")]
        Add = 0,
        [Description("修改")]
        Modify = 1,
        [Description("删除")]
        Delete = 2,
        [Description("默认")]
        View = 3
    }
}