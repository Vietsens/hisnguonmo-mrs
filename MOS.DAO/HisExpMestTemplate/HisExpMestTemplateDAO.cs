using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExpMestTemplate
{
    public partial class HisExpMestTemplateDAO : EntityBase
    {
        private HisExpMestTemplateGet GetWorker
        {
            get
            {
                return (HisExpMestTemplateGet)Worker.Get<HisExpMestTemplateGet>();
            }
        }
        public List<HIS_EXP_MEST_TEMPLATE> Get(HisExpMestTemplateSO search, CommonParam param)
        {
            List<HIS_EXP_MEST_TEMPLATE> result = new List<HIS_EXP_MEST_TEMPLATE>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_EXP_MEST_TEMPLATE GetById(long id, HisExpMestTemplateSO search)
        {
            HIS_EXP_MEST_TEMPLATE result = null;
            try
            {
                result = GetWorker.GetById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
