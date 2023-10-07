using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisDiseaseRelation
{
    partial class HisDiseaseRelationGet : EntityBase
    {
        public Dictionary<string, HIS_DISEASE_RELATION> GetDicByCode(HisDiseaseRelationSO search, CommonParam param)
        {
            Dictionary<string, HIS_DISEASE_RELATION> dic = new Dictionary<string, HIS_DISEASE_RELATION>();
            try
            {
                List<HIS_DISEASE_RELATION> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.DISEASE_RELATION_CODE))
                        {
                            dic.Add(item.DISEASE_RELATION_CODE, item);
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
