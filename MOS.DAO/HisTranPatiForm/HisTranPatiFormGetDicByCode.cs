using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisTranPatiForm
{
    partial class HisTranPatiFormGet : EntityBase
    {
        public Dictionary<string, HIS_TRAN_PATI_FORM> GetDicByCode(HisTranPatiFormSO search, CommonParam param)
        {
            Dictionary<string, HIS_TRAN_PATI_FORM> dic = new Dictionary<string, HIS_TRAN_PATI_FORM>();
            try
            {
                List<HIS_TRAN_PATI_FORM> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.TRAN_PATI_FORM_CODE))
                        {
                            dic.Add(item.TRAN_PATI_FORM_CODE, item);
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
