using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisExamServiceTemp
{
    partial class HisExamServiceTempGet : EntityBase
    {
        public Dictionary<string, HIS_EXAM_SERVICE_TEMP> GetDicByCode(HisExamServiceTempSO search, CommonParam param)
        {
            Dictionary<string, HIS_EXAM_SERVICE_TEMP> dic = new Dictionary<string, HIS_EXAM_SERVICE_TEMP>();
            try
            {
                List<HIS_EXAM_SERVICE_TEMP> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.EXAM_SERVICE_TEMP_CODE))
                        {
                            dic.Add(item.EXAM_SERVICE_TEMP_CODE, item);
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
