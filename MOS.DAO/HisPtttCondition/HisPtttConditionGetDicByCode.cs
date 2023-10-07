using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisPtttCondition
{
    partial class HisPtttConditionGet : EntityBase
    {
        public Dictionary<string, HIS_PTTT_CONDITION> GetDicByCode(HisPtttConditionSO search, CommonParam param)
        {
            Dictionary<string, HIS_PTTT_CONDITION> dic = new Dictionary<string, HIS_PTTT_CONDITION>();
            try
            {
                List<HIS_PTTT_CONDITION> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.PTTT_CONDITION_CODE))
                        {
                            dic.Add(item.PTTT_CONDITION_CODE, item);
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
