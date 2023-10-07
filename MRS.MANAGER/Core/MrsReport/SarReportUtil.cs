using Inventec.Common.Logging;
using Inventec.Common.WebApiClient;
using Inventec.Core;
using MRS.MANAGER.Base;
using SAR.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Core.MrsReport
{
    class SarReportUtil : BeanObjectBase
    {
        private const string URI_UPDATE_STT = "/api/SarReport/UpdateStt";
        private const string URI_CREATE = "/api/SarReport/Create";

        public SarReportUtil(CommonParam param)
            : base(param)
        {
        }

        public bool Update(SAR_REPORT data)
        {
            bool result = false;
            try
            {
                CommonParam paramUpdate = new CommonParam();
                var resultData = new SAR.MANAGER.Manager.SarReportManager(paramUpdate).Update(data);
                if (resultData != null)
                {
                    result = true;
                }
                else
                {
                    LogSystem.Info("Cap nhat trang thai bao cao that bai. Input:" + LogUtil.TraceData("data", data) + ", Output: " + LogUtil.TraceData("resultData", resultData));
                    CopyCommonParamInfo(paramUpdate);
                }
            }
            catch (ApiException ex)
            {
                LogSystem.Info(LogUtil.TraceData("StatusCode", ex.StatusCode));
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        public bool Create(SAR_REPORT data, ref SAR_REPORT resultData)
        {
            bool result = false;
            try
            {
                var dataResult = new SAR.MANAGER.Manager.SarReportManager(param).Create(data);

                if (dataResult != null)
                {
                    resultData = (SAR_REPORT)dataResult;
                    result = true;
                }
                else
                {
                    LogSystem.Error("SAR tao bao cao that bai");
                    CopyCommonParamInfo(param);
                }
            }
            catch (ApiException ex)
            {
                LogSystem.Info(LogUtil.TraceData("StatusCode", ex.StatusCode));
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
    }
}
