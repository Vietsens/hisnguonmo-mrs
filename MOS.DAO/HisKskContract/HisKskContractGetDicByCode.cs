using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisKskContract
{
    partial class HisKskContractGet : EntityBase
    {
        public Dictionary<string, HIS_KSK_CONTRACT> GetDicByCode(HisKskContractSO search, CommonParam param)
        {
            Dictionary<string, HIS_KSK_CONTRACT> dic = new Dictionary<string, HIS_KSK_CONTRACT>();
            try
            {
                List<HIS_KSK_CONTRACT> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.KSK_CONTRACT_CODE))
                        {
                            dic.Add(item.KSK_CONTRACT_CODE, item);
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
