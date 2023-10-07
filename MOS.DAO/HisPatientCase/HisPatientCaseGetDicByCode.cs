using Inventec.Common.Logging;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPatientCase
{
    partial class HisPatientCaseGet : EntityBase
    {
        public Dictionary<string, HIS_PATIENT_CASE> GetDicByCode(HisPatientCaseSO search, CommonParam param)
        {
            Dictionary<string, HIS_PATIENT_CASE> dic = new Dictionary<string, HIS_PATIENT_CASE>();
            try
            {
                List<HIS_PATIENT_CASE> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.PATIENT_CASE_CODE))
                        {
                            dic.Add(item.PATIENT_CASE_CODE, item);
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
