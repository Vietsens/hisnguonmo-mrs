using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisMedicineLine
{
    partial class HisMedicineLineGet : EntityBase
    {
        public Dictionary<string, HIS_MEDICINE_LINE> GetDicByCode(HisMedicineLineSO search, CommonParam param)
        {
            Dictionary<string, HIS_MEDICINE_LINE> dic = new Dictionary<string, HIS_MEDICINE_LINE>();
            try
            {
                List<HIS_MEDICINE_LINE> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.MEDICINE_LINE_CODE))
                        {
                            dic.Add(item.MEDICINE_LINE_CODE, item);
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
