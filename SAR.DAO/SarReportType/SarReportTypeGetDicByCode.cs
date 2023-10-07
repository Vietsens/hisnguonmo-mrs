using SAR.DAO.Base;
using SAR.DAO.StagingObject;
using SAR.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAR.DAO.SarReportType
{
    partial class SarReportTypeGet : EntityBase
    {
        public Dictionary<string, SAR_REPORT_TYPE> GetDicByCode(SarReportTypeSO search, CommonParam param)
        {
            Dictionary<string, SAR_REPORT_TYPE> dic = new Dictionary<string, SAR_REPORT_TYPE>();
            try
            {
                List<SAR_REPORT_TYPE> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.REPORT_TYPE_CODE))
                        {
                            dic.Add(item.REPORT_TYPE_CODE, item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => search), search) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => param), param), LogType.Error);
                LogSystem.Error(ex);
                dic.Clear();
            }
            return dic;
        }
    }
}
