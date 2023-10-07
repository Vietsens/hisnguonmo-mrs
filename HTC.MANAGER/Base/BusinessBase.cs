using Inventec.Core;
using System;

namespace HTC.MANAGER.Base
{
    public abstract class BusinessBase : EntityBase
    {
        protected CommonParam param { get; set; }

        public BusinessBase()
            : base()
        {
            param = new CommonParam();
            try
            {
                UserName = Inventec.Token.ResourceSystem.ResourceTokenManager.GetLoginName();
            }
            catch (Exception)
            {
            }
        }

        public BusinessBase(CommonParam paramBusiness)
            : base()
        {
            param = (paramBusiness != null ? paramBusiness : new CommonParam());
            try
            {
                UserName = Inventec.Token.ResourceSystem.ResourceTokenManager.GetLoginName();
            }
            catch (Exception)
            {
            }
        }

        public bool HasException()
        {
            return param.HasException;
        }

        public void CopyCommonParamInfoGet(CommonParam paramSource)
        {
            try
            {
                param.Start = paramSource.Start;
                param.Limit = paramSource.Limit;
                param.Count = paramSource.Count;
                param.HasException = paramSource.HasException;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public void CopyCommonParamInfo(CommonParam paramSource)
        {
            try
            {
                if (paramSource.BugCodes != null && paramSource.BugCodes.Count > 0) param.BugCodes.AddRange(paramSource.BugCodes);
                if (paramSource.Messages != null && paramSource.Messages.Count > 0) param.Messages.AddRange(paramSource.Messages);
                param.Start = paramSource.Start;
                param.Limit = paramSource.Limit;
                param.Count = paramSource.Count;
                param.HasException = paramSource.HasException;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public void CopyCommonParamInfo(BusinessBase fromObject)
        {
            try
            {
                if (fromObject.param != null)
                {
                    if (fromObject.param.BugCodes != null && fromObject.param.BugCodes.Count > 0) param.BugCodes.AddRange(fromObject.param.BugCodes);
                    if (fromObject.param.Messages != null && fromObject.param.Messages.Count > 0) param.Messages.AddRange(fromObject.param.Messages);
                    param.Start = fromObject.param.Start;
                    param.Limit = fromObject.param.Limit;
                    param.Count = fromObject.param.Count;
                    param.HasException = fromObject.param.HasException;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected void TroubleCheck()
        {
            try
            {
                if (param.HasException || (param.BugCodes != null && param.BugCodes.Count > 0))
                {
                    //MethodName = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;
                    //TroubleCache.Add(GetInfoProcess() + (param.HasException ? "param.HasException." : "") + param.GetBugCode());
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected ApiResultObject<T> PackResult<T>(T resultData)
        {
            ApiResultObject<T> result = new ApiResultObject<T>();
            try
            {
                result.SetValue(resultData, Inventec.Core.Util.DecisionApiResult(resultData), param);
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
