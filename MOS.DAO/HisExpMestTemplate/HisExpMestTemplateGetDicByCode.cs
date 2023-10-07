using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisExpMestTemplate
{
    partial class HisExpMestTemplateGet : EntityBase
    {
        public Dictionary<string, HIS_EXP_MEST_TEMPLATE> GetDicByCode(HisExpMestTemplateSO search, CommonParam param)
        {
            Dictionary<string, HIS_EXP_MEST_TEMPLATE> dic = new Dictionary<string, HIS_EXP_MEST_TEMPLATE>();
            try
            {
                List<HIS_EXP_MEST_TEMPLATE> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.EXP_MEST_TEMPLATE_CODE))
                        {
                            dic.Add(item.EXP_MEST_TEMPLATE_CODE, item);
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
