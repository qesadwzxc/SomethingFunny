using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MvcApi.Models;
using MvcApi.Business;
using System.Reflection;
using MvcApi.IBusiness;
using MvcApi.DataAccess;

namespace MvcApi.Controllers
{
    public class UserController : ApiController
    {
        IApiTest provider = new ApiTest();
        public IEnumerable<NonceModel> GetAllContact()
        {
            return provider.GetListAll();
        }

        public NonceModel GetNonce(int id)
        {
            NonceModel contact = provider.GetModelByID(id);
            if (contact == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            contact.Nonce = contact.Nonce.ToString().Replace("-", "");
            contact.CreateTime = contact.CreateTime.Date;
            return contact;
        }

        public string PostUser(NonceModel model)
        {
            //model = provider.Add(model);
            Access_Sys_Nonce asn = new Access_Sys_Nonce();
            NonceModel nm = new NonceModel() { ID = 0 };
            for (int i = 0; i < 10;i++ )
            {
                nm.Nonce = (Guid.NewGuid()).ToString();
                nm.CreateTime = DateTime.Now;
                bool result = asn.CreateNonce(nm);
            }
            return "What?";
        }

        public void PutUser(NonceModel model)
        {
            provider.Update(model);
        }

        public void DeleteUser(int id)
        {
            provider.Remove(id);
        }
    }
}
