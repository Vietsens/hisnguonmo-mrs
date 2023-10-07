using Inventec.Common.Logging;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisActiveIngredient
{
    partial class HisActiveIngredientGet : EntityBase
    {
        public Dictionary<string, HIS_ACTIVE_INGREDIENT> GetDicByCode(HisActiveIngredientSO search, CommonParam param)
        {
            Dictionary<string, HIS_ACTIVE_INGREDIENT> dic = new Dictionary<string, HIS_ACTIVE_INGREDIENT>();
            try
            {
                List<HIS_ACTIVE_INGREDIENT> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.ACTIVE_INGREDIENT_CODE))
                        {
                            dic.Add(item.ACTIVE_INGREDIENT_CODE, item);
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
