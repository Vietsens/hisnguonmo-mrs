using Inventec.Common.Logging;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccountBook
{
    partial class HisAccountBookGet : EntityBase
    {
        public Dictionary<string, HIS_ACCOUNT_BOOK> GetDicByCode(HisAccountBookSO search, CommonParam param)
        {
            Dictionary<string, HIS_ACCOUNT_BOOK> dic = new Dictionary<string, HIS_ACCOUNT_BOOK>();
            try
            {
                List<HIS_ACCOUNT_BOOK> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.ACCOUNT_BOOK_CODE))
                        {
                            dic.Add(item.ACCOUNT_BOOK_CODE, item);
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
