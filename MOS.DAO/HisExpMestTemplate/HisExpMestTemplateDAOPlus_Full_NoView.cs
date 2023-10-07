using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExpMestTemplate
{
    public partial class HisExpMestTemplateDAO : EntityBase
    {
        public HIS_EXP_MEST_TEMPLATE GetByCode(string code, HisExpMestTemplateSO search)
        {
            HIS_EXP_MEST_TEMPLATE result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public Dictionary<string, HIS_EXP_MEST_TEMPLATE> GetDicByCode(HisExpMestTemplateSO search, CommonParam param)
        {
            Dictionary<string, HIS_EXP_MEST_TEMPLATE> result = new Dictionary<string, HIS_EXP_MEST_TEMPLATE>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }
    }
}
