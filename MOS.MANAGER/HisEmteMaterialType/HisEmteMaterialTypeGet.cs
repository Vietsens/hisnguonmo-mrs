using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEmteMaterialType
{
    class HisEmteMaterialTypeGet : GetBase
    {
        internal HisEmteMaterialTypeGet()
            : base()
        {

        }

        internal HisEmteMaterialTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EMTE_MATERIAL_TYPE> Get(HisEmteMaterialTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEmteMaterialTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EMTE_MATERIAL_TYPE> GetView(HisEmteMaterialTypeViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEmteMaterialTypeDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EMTE_MATERIAL_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisEmteMaterialTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EMTE_MATERIAL_TYPE GetById(long id, HisEmteMaterialTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEmteMaterialTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_EMTE_MATERIAL_TYPE GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisEmteMaterialTypeViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EMTE_MATERIAL_TYPE GetViewById(long id, HisEmteMaterialTypeViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEmteMaterialTypeDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EMTE_MATERIAL_TYPE> GetByMaterialTypeId(long id)
        {
            try
            {
                HisEmteMaterialTypeFilterQuery filter = new HisEmteMaterialTypeFilterQuery();
                filter.MATERIAL_TYPE_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EMTE_MATERIAL_TYPE> GetByExpMestTemplateId(long id)
        {
            try
            {
                HisEmteMaterialTypeFilterQuery filter = new HisEmteMaterialTypeFilterQuery();
                filter.EXP_MEST_TEMPLATE_ID = id;
                return this.Get(filter);
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
