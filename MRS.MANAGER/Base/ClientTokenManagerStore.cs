using Inventec.Common.Logging;
using Inventec.Common.WebApiClient;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Base
{
    public class ClientTokenManagerStore
    {
        private static Inventec.Token.ClientSystem.ClientTokenManager mosClientTokenManager;
        public static Inventec.Token.ClientSystem.ClientTokenManager ClientTokenManager
        {
            get
            {
                if (mosClientTokenManager == null)
                {
                    mosClientTokenManager = new Inventec.Token.ClientSystem.ClientTokenManager(ApplicationConfig.APPLICATION_CODE__MRS);
                }
                return mosClientTokenManager;
            }
            set
            {
                mosClientTokenManager = value;
            }
        }

        public static Dictionary<string, object> DicToken = new Dictionary<string, object>();

        public static T CreatePostRequest<T>(string requestUri, Inventec.Common.WebApiClient.ApiConsumer consumer, CommonParam commonParam, object data)
        {
            T result = default(T);
            try
            {
                Inventec.Core.ApiResultObject<T> rs = consumer.Post<Inventec.Core.ApiResultObject<T>>(requestUri, commonParam, data);
                if (rs != null)
                {
                    if (rs.Param != null)
                    {
                        commonParam.Messages.AddRange(rs.Param.Messages);
                        commonParam.BugCodes.AddRange(rs.Param.BugCodes);
                    }
                    if (rs.Success) result = (rs.Data);
                }
            }
            catch (ApiException ex)
            {
                Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => ex.StatusCode), ex.StatusCode));
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                commonParam.HasException = true;
                result = default(T);
            }
            return result;
        }

        public static T CreateGetRequest<T>(string requestUri, Inventec.Common.WebApiClient.ApiConsumer consumer, CommonParam commonParam, object filter)
        {
            T result = default(T);
            try
            {
                Inventec.Core.ApiResultObject<T> rs = consumer.Get<Inventec.Core.ApiResultObject<T>>(requestUri, commonParam, filter);
                if (rs != null)
                {
                    if (rs.Param != null)
                    {
                        commonParam.Messages.AddRange(rs.Param.Messages);
                        commonParam.BugCodes.AddRange(rs.Param.BugCodes);
                    }
                    if (rs.Success) result = (rs.Data);
                }
            }
            catch (ApiException ex)
            {
                Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => ex.StatusCode), ex.StatusCode));
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                commonParam.HasException = true;
                result = default(T);
            }
            return result;
        }
    }
}
