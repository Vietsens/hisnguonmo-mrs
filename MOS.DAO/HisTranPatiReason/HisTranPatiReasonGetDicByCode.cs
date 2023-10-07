using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisTranPatiReason
{
    partial class HisTranPatiReasonGet : EntityBase
    {
        public Dictionary<string, HIS_TRAN_PATI_REASON> GetDicByCode(HisTranPatiReasonSO search, CommonParam param)
        {
            Dictionary<string, HIS_TRAN_PATI_REASON> dic = new Dictionary<string, HIS_TRAN_PATI_REASON>();
            try
            {
                List<HIS_TRAN_PATI_REASON> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.TRAN_PATI_REASON_CODE))
                        {
                            dic.Add(item.TRAN_PATI_REASON_CODE, item);
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
