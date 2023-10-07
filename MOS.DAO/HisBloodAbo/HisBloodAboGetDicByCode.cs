using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisBloodAbo
{
    partial class HisBloodAboGet : EntityBase
    {
        public Dictionary<string, HIS_BLOOD_ABO> GetDicByCode(HisBloodAboSO search, CommonParam param)
        {
            Dictionary<string, HIS_BLOOD_ABO> dic = new Dictionary<string, HIS_BLOOD_ABO>();
            try
            {
                List<HIS_BLOOD_ABO> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.BLOOD_ABO_CODE))
                        {
                            dic.Add(item.BLOOD_ABO_CODE, item);
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
