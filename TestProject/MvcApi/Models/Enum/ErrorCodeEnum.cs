using System.ComponentModel;

namespace MvcApi.Models.Enum
{
    public enum ErrorCodeEnum
    {
        #region 系统级错误码
        /// <summary>
        /// 错误的请求KEY
        /// </summary>
        [Description("错误的请求KEY")]
        C_10001=10001,
        /// <summary>
        /// 该KEY无请求权限
        /// </summary>
        [Description("该KEY无请求权限")]
        C_10002 = 10002,
        /// <summary>
        /// KEY过期
        /// </summary>
        [Description("KEY过期")]
        C_10003 = 10003,
        /// <summary>
        /// 错误的OPENID
        /// </summary>
        [Description("错误的OPENID")]
        C_10004 = 10004,
        /// <summary>
        /// 应用未审核超时，请提交认证
        /// </summary>
        [Description("应用未审核超时，请提交认证")]
        C_10005 = 10005,
        /// <summary>
        /// 未知的请求源
        /// </summary>
        [Description("未知的请求源")]
        C_10007 = 10007,
        /// <summary>
        /// 被禁止的IP
        /// </summary>
        [Description("被禁止的IP")]
        C_10008 = 10008,
        /// <summary>
        /// 被禁止的KEY
        /// </summary>
        [Description("被禁止的KEY")]
        C_10009 = 10009,
        /// <summary>
        /// 当前IP请求超过限制
        /// </summary>
        [Description("当前IP请求超过限制")]
        C_10011 = 10011,
        /// <summary>
        /// 请求超过次数限制
        /// </summary>
        [Description("请求超过次数限制")]
        C_10012 = 10012,
        /// <summary>
        /// 测试KEY超过请求限制
        /// </summary>
        [Description("测试KEY超过请求限制")]
        C_10013 = 10013,
        /// <summary>
        /// 系统内部异常
        /// </summary>
        [Description("系统内部异常")]
        C_10014 = 10014,
        /// <summary>
        /// 接口维护
        /// </summary>
        [Description("接口维护")]
        C_10020 = 10020,
        /// <summary>
        /// 接口停用
        /// </summary>
        [Description("接口停用")]
        C_10021 = 10021,
        #endregion

        #region 天气预报
        /// <summary>
        /// 错误的查询城市名
        /// </summary>
        [Description("错误的查询城市名")]
        C_207301 = 207301,
        /// <summary>
        /// 查询不到该城市的相关信息
        /// </summary>
        [Description("查询不到该城市的相关信息")]
        C_207302 = 207302,
        /// <summary>
        /// 网络错误，请重试
        /// </summary>
        [Description("网络错误，请重试")]
        C_207303 = 207303,
        #endregion

        #region 股票数据
        /// <summary>
        /// 参数错误
        /// </summary>
        [Description("参数错误")]
        C_202101 = 202101,
        /// <summary>
        /// 查询不到结果
        /// </summary>
        [Description("查询不到结果")]
        C_202102 = 202102,
        #endregion

        #region 火车时刻表
        /// <summary>
        /// 火车班次不能为空
        /// </summary>
        [Description("火车班次不能为空")]
        C_207901 = 207901,
        /// <summary>
        /// 查询不到火车该班次相关信息
        /// </summary>
        [Description("查询不到火车该班次相关信息")]
        C_207902 = 207902,
        /// <summary>
        /// 网络错误，请重试
        /// </summary>
        [Description("网络错误，请重试")]
        C_207903 = 207903,
        /// <summary>
        /// 火车始发终点站不能为空
        /// </summary>
        [Description("火车始发终点站不能为空")]
        C_207904 = 207904,
        /// <summary>
        /// 查询不到火车始发站相关信息
        /// </summary>
        [Description("查询不到火车始发站相关信息")]
        C_207905 = 207905,
        #endregion

        #region 问答机器人
        /// <summary>
        /// 网络错误，请重试
        /// </summary>
        [Description("网络错误，请重试")]
        C_211200 = 211200,
        /// <summary>
        /// 请提供info参数，即需要查询的内容
        /// </summary>
        [Description("请提供info参数，即需要查询的内容")]
        C_211201 = 211201,
        /// <summary>
        /// info参数的值不可超过30个字符
        /// </summary>
        [Description("info参数的值不可超过30个字符")]
        C_211202 = 211202,
        /// <summary>
        /// 内部程序错误，请联系客服
        /// </summary>
        [Description("内部程序错误，请联系客服")]
        C_211203 = 211203,
        /// <summary>
        /// 经度信息应为数字，如东经116.234632（小数点后保留6位），需要写为116234632
        /// </summary>
        [Description("经度信息应为数字")]
        C_211204 = 211204,
        /// <summary>
        /// 纬度信息应为数字，如北纬40.234632（小数点后保留6位），需要写为40234632
        /// </summary>
        [Description("纬度信息应为数字")]
        C_211205 = 211205
        #endregion
    }
}