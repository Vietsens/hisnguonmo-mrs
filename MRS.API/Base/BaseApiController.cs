using Inventec.Core;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MRS.API.Base
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    //[LicenseFilter]
    //[AllowAnonymous]
    public class BaseApiController : ApiController
    {
        protected CommonParam commonParam;

        protected ApiResultObject<T> PackResult<T>(T resultData)
        {
            ApiResultObject<T> result = new ApiResultObject<T>();
            try
            {
                result.SetValue(resultData, Inventec.Core.Util.DecisionApiResult(resultData), commonParam);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                Inventec.Common.Logging.LogSystem.Error(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => resultData), resultData));
                result = new ApiResultObject<T>(default(T), false);
            }
            return result;
        }
    }
}
