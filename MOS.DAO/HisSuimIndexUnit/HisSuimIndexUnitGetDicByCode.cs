using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisSuimIndexUnit
{
    partial class HisSuimIndexUnitGet : EntityBase
    {
        public Dictionary<string, HIS_SUIM_INDEX_UNIT> GetDicByCode(HisSuimIndexUnitSO search, CommonParam param)
        {
            Dictionary<string, HIS_SUIM_INDEX_UNIT> dic = new Dictionary<string, HIS_SUIM_INDEX_UNIT>();
            try
            {
                List<HIS_SUIM_INDEX_UNIT> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.SUIM_INDEX_UNIT_CODE))
                        {
                            dic.Add(item.SUIM_INDEX_UNIT_CODE, item);
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
