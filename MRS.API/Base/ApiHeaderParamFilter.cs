using Inventec.Common.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MRS.API.Base
{
    public class ApiHeaderParamFilter : ActionFilterAttribute
    {
        Type _type;
        object _data;
        string _queryStringKey;
        public ApiHeaderParamFilter(Type type, string queryStringKey, object data)
        {
            _type = type;
            _queryStringKey = queryStringKey;
            _data = data;
        }
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                LogSystem.Debug(string.Format("User: {0}, Controller: {1}, Action: {2}", Inventec.Token.ResourceSystem.ResourceTokenManager.GetLoginName(), actionContext.ControllerContext.ControllerDescriptor.ControllerName, actionContext.ActionDescriptor.ActionName));

                var json = actionContext.Request.Headers.GetValues(_queryStringKey).FirstOrDefault(); ;
                if (json != null)
                {
                    LogSystem.Debug(string.Format("Input: {0}", json));
                    this._data = JsonConvert.DeserializeObject(json, _type);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                throw ex;
            }
        }
    }
}