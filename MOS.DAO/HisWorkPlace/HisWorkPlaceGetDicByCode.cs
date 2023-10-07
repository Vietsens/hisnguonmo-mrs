using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisWorkPlace
{
    partial class HisWorkPlaceGet : EntityBase
    {
        public Dictionary<string, HIS_WORK_PLACE> GetDicByCode(HisWorkPlaceSO search, CommonParam param)
        {
            Dictionary<string, HIS_WORK_PLACE> dic = new Dictionary<string, HIS_WORK_PLACE>();
            try
            {
                List<HIS_WORK_PLACE> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.WORK_PLACE_CODE))
                        {
                            dic.Add(item.WORK_PLACE_CODE, item);
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
