using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisTreatmentResult
{
    partial class HisTreatmentResultGet : EntityBase
    {
        public Dictionary<string, HIS_TREATMENT_RESULT> GetDicByCode(HisTreatmentResultSO search, CommonParam param)
        {
            Dictionary<string, HIS_TREATMENT_RESULT> dic = new Dictionary<string, HIS_TREATMENT_RESULT>();
            try
            {
                List<HIS_TREATMENT_RESULT> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.TREATMENT_RESULT_CODE))
                        {
                            dic.Add(item.TREATMENT_RESULT_CODE, item);
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
