using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisBedType
{
    partial class HisBedTypeGet : EntityBase
    {
        public Dictionary<string, HIS_BED_TYPE> GetDicByCode(HisBedTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_BED_TYPE> dic = new Dictionary<string, HIS_BED_TYPE>();
            try
            {
                List<HIS_BED_TYPE> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.BED_TYPE_CODE))
                        {
                            dic.Add(item.BED_TYPE_CODE, item);
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
