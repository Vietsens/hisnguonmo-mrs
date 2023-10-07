using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisServiceReqStt
{
    partial class HisServiceReqSttGet : EntityBase
    {
        public Dictionary<string, HIS_SERVICE_REQ_STT> GetDicByCode(HisServiceReqSttSO search, CommonParam param)
        {
            Dictionary<string, HIS_SERVICE_REQ_STT> dic = new Dictionary<string, HIS_SERVICE_REQ_STT>();
            try
            {
                List<HIS_SERVICE_REQ_STT> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.SERVICE_REQ_STT_CODE))
                        {
                            dic.Add(item.SERVICE_REQ_STT_CODE, item);
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
