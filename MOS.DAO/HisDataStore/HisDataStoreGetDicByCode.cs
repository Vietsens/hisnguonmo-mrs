using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisDataStore
{
    partial class HisDataStoreGet : EntityBase
    {
        public Dictionary<string, HIS_DATA_STORE> GetDicByCode(HisDataStoreSO search, CommonParam param)
        {
            Dictionary<string, HIS_DATA_STORE> dic = new Dictionary<string, HIS_DATA_STORE>();
            try
            {
                List<HIS_DATA_STORE> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.DATA_STORE_CODE))
                        {
                            dic.Add(item.DATA_STORE_CODE, item);
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
