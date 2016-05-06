using MvcApi.Models;
using MvcApi.IBusiness;
using MvcApi.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static System.Web.HttpUtility;
using System.Web.Http;
using System.Data;
using VinCode.Web;
using System.Configuration;

namespace MvcApi.Business
{
    public class ApiTest : IApiTest
    {
        private List<NonceModel> listModel = new List<NonceModel>();
        //public ApiTest()
        //{
        //    Access_Sys_Nonce asn = new Access_Sys_Nonce();
        //    DataTable dt = asn.AllNonce();
        //    DateTime date;
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        DateTime.TryParse(dt.Rows[i]["CreateTime"].ToString(),out date);
        //        listModel.Add(new NonceModel() { ID = Convert.ToInt32(dt.Rows[i]["ID"]), Nonce = dt.Rows[i]["Nonce"].ToString(), CreateTime = date });
        //    }
        //}

        //public IEnumerable<NonceModel> GetListAll()
        //{
        //    return listModel;
        //}

        //public NonceModel GetModelByID(int id)
        //{
        //    NonceModel model = listModel.FirstOrDefault<NonceModel>(p => p.ID == id);
        //    if (model == null)
        //    {
        //        throw new HttpResponseException(HttpStatusCode.NotFound);
        //    }
        //    return model;
        //}

        //public IEnumerable<NonceModel> GetListBySex(string nonce)
        //{
        //    return listModel.Where(p => p.Nonce == nonce);
        //}

        //public NonceModel Add(NonceModel model)
        //{
        //    if (model == null)
        //    {
        //        throw new HttpResponseException(HttpStatusCode.MethodNotAllowed);
        //    }
        //    int maxID = listModel.Max(p => p.ID);
        //    model.ID = maxID + 1;
        //    listModel.Add(model);
        //    return model;
        //}

        //public void Remove(int id)
        //{
        //    listModel.RemoveAll(p => p.ID == id);
        //}

        //public void Update(NonceModel model)
        //{
        //    if (model == null)
        //    {
        //        throw new HttpResponseException(HttpStatusCode.MethodNotAllowed);
        //    }

        //    listModel.RemoveAll(p => p.ID==model.ID);
        //    listModel.Add(model);
        //}

        public CommonResponse<RobotRes> ChatWithRobot(string info)
        {
            string url = $"http://op.juhe.cn/robot/index?info={info}&key={ConfigurationManager.AppSettings["AppKey_Robot"]}";
            CommonResponse<RobotRes> response = WebRequestHelper.Get<CommonResponse<RobotRes>>(url);
            return response;
        }

        public CommonResponse<TrainRes> TrainTimes(string trainId)
        {
            string url = $"http://op.juhe.cn/onebox/train/query?key={ConfigurationManager.AppSettings["AppKey_Train"]}&train={trainId}";
            CommonResponse<TrainRes> response = WebRequestHelper.Get<CommonResponse<TrainRes>>(url);
            return response;
        }

        public CommonResponse<WeatherRes> CityWeather(string cityname)
        {
            string url = $"http://op.juhe.cn/onebox/weather/query?cityname={UrlEncode(cityname)}&key={ConfigurationManager.AppSettings["AppKey_Weather"]}";
            CommonResponse<WeatherRes> response = WebRequestHelper.Get<CommonResponse<WeatherRes>>(url);
            return response;
        }
    }
}