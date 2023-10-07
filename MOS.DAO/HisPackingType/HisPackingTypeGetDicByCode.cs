using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisPackingType
{
    partial class HisPackingTypeGet : EntityBase
    {
        public Dictionary<string, HIS_PACKING_TYPE> GetDicByCode(HisPackingTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_PACKING_TYPE> dic = new Dictionary<string, HIS_PACKING_TYPE>();
            try
            {
                List<HIS_PACKING_TYPE> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.PACKING_TYPE_CODE))
                        {
                            dic.Add(item.PACKING_TYPE_CODE, item);
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
