using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestTemplate
{
    class HisExpMestTemplateGet : GetBase
    {
        internal HisExpMestTemplateGet()
            : base()
        {

        }

        internal HisExpMestTemplateGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EXP_MEST_TEMPLATE> Get(HisExpMestTemplateFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestTemplateDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_TEMPLATE GetById(long id)
        {
            try
            {
                return GetById(id, new HisExpMestTemplateFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_TEMPLATE GetById(long id, HisExpMestTemplateFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestTemplateDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_TEMPLATE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisExpMestTemplateFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_TEMPLATE GetByCode(string code, HisExpMestTemplateFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestTemplateDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
