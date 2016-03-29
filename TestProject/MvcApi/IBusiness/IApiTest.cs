using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcApi.Models;

namespace MvcApi.IBusiness
{
    public interface IApiTest
    {
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        IEnumerable<NonceModel> GetListAll();
        /// <summary>
        /// 通过ID查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        NonceModel GetModelByID(int id);
        /// <summary>
        /// 通过性别查找
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        IEnumerable<NonceModel> GetListBySex(string sex);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        NonceModel Add(NonceModel model);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        void Remove(int id);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        void Update(NonceModel model);
    }
}