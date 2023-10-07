using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisMedicineUseForm
{
    partial class HisMedicineUseFormGet : EntityBase
    {
        public Dictionary<string, HIS_MEDICINE_USE_FORM> GetDicByCode(HisMedicineUseFormSO search, CommonParam param)
        {
            Dictionary<string, HIS_MEDICINE_USE_FORM> dic = new Dictionary<string, HIS_MEDICINE_USE_FORM>();
            try
            {
                List<HIS_MEDICINE_USE_FORM> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.MEDICINE_USE_FORM_CODE))
                        {
                            dic.Add(item.MEDICINE_USE_FORM_CODE, item);
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
