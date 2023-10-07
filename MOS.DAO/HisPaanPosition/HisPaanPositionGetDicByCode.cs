using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisPaanPosition
{
    partial class HisPaanPositionGet : EntityBase
    {
        public Dictionary<string, HIS_PAAN_POSITION> GetDicByCode(HisPaanPositionSO search, CommonParam param)
        {
            Dictionary<string, HIS_PAAN_POSITION> dic = new Dictionary<string, HIS_PAAN_POSITION>();
            try
            {
                List<HIS_PAAN_POSITION> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.PAAN_POSITION_CODE))
                        {
                            dic.Add(item.PAAN_POSITION_CODE, item);
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
