using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisPtttMethod
{
    partial class HisPtttMethodGet : EntityBase
    {
        public Dictionary<string, HIS_PTTT_METHOD> GetDicByCode(HisPtttMethodSO search, CommonParam param)
        {
            Dictionary<string, HIS_PTTT_METHOD> dic = new Dictionary<string, HIS_PTTT_METHOD>();
            try
            {
                List<HIS_PTTT_METHOD> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.PTTT_METHOD_CODE))
                        {
                            dic.Add(item.PTTT_METHOD_CODE, item);
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
