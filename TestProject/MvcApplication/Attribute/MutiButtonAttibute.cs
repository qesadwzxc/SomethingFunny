using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication.Attribute
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class MutiButtonAttribute : ActionNameSelectorAttribute
    {
        public string Name { get; private set; }

        public MutiButtonAttribute(string name)
        {
            this.Name = name;
        }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, System.Reflection.MethodInfo methodInfo)
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new NotImplementedException();
            }
            return controllerContext.HttpContext.Request.Form.AllKeys.Contains(this.Name);
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class MutiButtonV2Attribute : ActionNameSelectorAttribute
    {
        public string Name { get; set; }
        public string Argument { get; set; }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            var key = ButtonKeyFrom(controllerContext);
            var keyIsValid = IsValid(key);

            if (keyIsValid)
            {
                UpdateValueProviderIn(controllerContext, ValueFrom(key));
            }

            return keyIsValid;
        }

        private string ButtonKeyFrom(ControllerContext controllerContext)
        {
            var keys = controllerContext.HttpContext.Request.Params.AllKeys;
            return keys.FirstOrDefault(KeyStartsWithButtonName);
        }

        private static bool IsValid(string key)
        {
            return key != null;
        }

        private static string ValueFrom(string key)
        {
            var parts = key.Split(":".ToCharArray());
            return parts.Length < 2 ? null : parts[1];
        }

        private void UpdateValueProviderIn(ControllerContext controllerContext, string value)
        {
            if (string.IsNullOrEmpty(Argument)) return;
            controllerContext.RouteData.Values[this.Argument] = value;
        }

        private bool KeyStartsWithButtonName(string key)
        {
            return key.StartsWith(Name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}