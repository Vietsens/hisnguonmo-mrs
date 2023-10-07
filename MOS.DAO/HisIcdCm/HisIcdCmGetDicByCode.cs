using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisIcdCm
{
    partial class HisIcdCmGet : EntityBase
    {
        public Dictionary<string, HIS_ICD_CM> GetDicByCode(HisIcdCmSO search, CommonParam param)
        {
            Dictionary<string, HIS_ICD_CM> dic = new Dictionary<string, HIS_ICD_CM>();
            try
            {
                List<HIS_ICD_CM> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.ICD_CM_CODE))
                        {
                            dic.Add(item.ICD_CM_CODE, item);
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
