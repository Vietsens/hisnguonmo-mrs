using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisDepositReq
{
    partial class HisDepositReqGet : EntityBase
    {
        public Dictionary<string, HIS_DEPOSIT_REQ> GetDicByCode(HisDepositReqSO search, CommonParam param)
        {
            Dictionary<string, HIS_DEPOSIT_REQ> dic = new Dictionary<string, HIS_DEPOSIT_REQ>();
            try
            {
                List<HIS_DEPOSIT_REQ> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.DEPOSIT_REQ_CODE))
                        {
                            dic.Add(item.DEPOSIT_REQ_CODE, item);
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
