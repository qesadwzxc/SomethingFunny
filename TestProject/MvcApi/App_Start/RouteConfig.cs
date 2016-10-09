using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcApi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }

    public class Categoty
    {
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
    }

    public class CustomRoute : RouteBase
    {
        public List<Categoty> Category
        {
            get
            {
                List<Categoty> c = new List<Categoty>();
                c.Add(new Categoty() { CategoryName = "ios", CategoryId = 1 });
                c.Add(new Categoty() { CategoryName = "android", CategoryId = 2 });
                c.Add(new Categoty() { CategoryName = "windows", CategoryId = 3 });
                return c;
            }
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var virtualPath = httpContext.Request.AppRelativeCurrentExecutionFilePath + httpContext.Request.PathInfo;//获取相对路径

            virtualPath = virtualPath.Substring(2).Trim('/');//此时URL会是～/ca-categoryname，截取后面的ca-categoryname

            if (!virtualPath.StartsWith("ca-"))//判断是否是我们需要处理的URL，不是则返回null，匹配将会继续进行。
                return null;

            var categoryname = virtualPath.Split('-').Last();//截取ca-前缀后的分类名称

            //尝试根据分类名称获取相应分类，忽略大小写
            var category = Category.Find(c => c.CategoryName.Equals(categoryname, StringComparison.OrdinalIgnoreCase));

            if (category == null)//如果分类是null，可能不是我们要处理的URL，返回null，让匹配继续进行
                return null;

            //TODO:替换路由规则为正则。

            //至此可以肯定是我们要处理的URL了
            var data = new RouteData(this, new MvcRouteHandler());//声明一个RouteData，添加相应的路由值
            data.Values.Add("controller", "Category");
            data.Values.Add("action", "ShowCategory");
            data.Values.Add("id", category.CategoryId);

            return data;//返回这个路由值将调用CategoryController.ShowCategory(category.CategoeyID)方法。匹配终止
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            //判断请求是否来源于CategoryController.Showcategory(string id),不是则返回null,让匹配继续
            int categoryId = 0;

            if (values["id"] == null && !int.TryParse(values["id"].ToString(), out categoryId))//路由信息中缺少参数id，不是我们要处理的请求，返回null
                return null;

            //请求不是CategoryController发起的，不是我们要处理的请求，返回null
            if (!values.ContainsKey("controller") || !values["controller"].ToString().Equals("category", StringComparison.OrdinalIgnoreCase))
                return null;
            //请求不是CategoryController.Showcategory(string id)发起的，不是我们要处理的请求，返回null
            if (!values.ContainsKey("action") || !values["action"].ToString().Equals("showcategory", StringComparison.OrdinalIgnoreCase))
                return null;

            //至此，我们可以确定请求是CategoryController.Showcategory(string id)发起的，生成相应的URL并返回
            var category = Category.Find(c => c.CategoryId == categoryId);

            if (category == null)
                throw new ArgumentNullException("category");//找不到分类抛出异常

            var path = "ca-" + category.CategoryName.Trim();//生成URL

            return new VirtualPathData(this, path.ToLowerInvariant());
        }
    }
}