using Inventec.Common.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MRS.API.Base
{
    public class ApiParamFilter : ActionFilterAttribute
    {
        Type _type;
        string _queryStringKey;
        public ApiParamFilter(Type type, string queryStringKey)
        {
            _type = type;
            _queryStringKey = queryStringKey;
        }
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                LogSystem.Debug(string.Format("User: {0}, Controller: {1}, Action: {2}", Inventec.Token.ResourceSystem.ResourceTokenManager.GetLoginName(), actionContext.ControllerContext.ControllerDescriptor.ControllerName, actionContext.ActionDescriptor.ActionName));
                var json = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query)[_queryStringKey];
                if (json != null)
                {
                    json = json.Replace(' ', '+');
                    json = Encoding.UTF8.GetString(Convert.FromBase64String(json));
                    LogSystem.Debug(string.Format("Input: {0}", json));
                    JsonSerializerSettings setting = new JsonSerializerSettings { Error = HandleDeserializationError };
                    actionContext.ActionArguments[_queryStringKey] = JsonConvert.DeserializeObject(json, _type, setting);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                throw ex;
            }
        }

        public static void HandleDeserializationError(object sender, ErrorEventArgs errorArgs)
        {
            errorArgs.ErrorContext.Handled = true;
        }
    }
}