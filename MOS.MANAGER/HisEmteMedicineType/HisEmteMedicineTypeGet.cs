using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEmteMedicineType
{
    class HisEmteMedicineTypeGet : GetBase
    {
        internal HisEmteMedicineTypeGet()
            : base()
        {

        }

        internal HisEmteMedicineTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EMTE_MEDICINE_TYPE> Get(HisEmteMedicineTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEmteMedicineTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EMTE_MEDICINE_TYPE> GetView(HisEmteMedicineTypeViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEmteMedicineTypeDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EMTE_MEDICINE_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisEmteMedicineTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EMTE_MEDICINE_TYPE GetById(long id, HisEmteMedicineTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEmteMedicineTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_EMTE_MEDICINE_TYPE GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisEmteMedicineTypeViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EMTE_MEDICINE_TYPE GetViewById(long id, HisEmteMedicineTypeViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEmteMedicineTypeDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EMTE_MEDICINE_TYPE> GetByMedicineTypeId(long id)
        {
            try
            {
                HisEmteMedicineTypeFilterQuery filter = new HisEmteMedicineTypeFilterQuery();
                filter.MEDICINE_TYPE_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EMTE_MEDICINE_TYPE> GetByExpMestTemplateId(long id)
        {
            try
            {
                HisEmteMedicineTypeFilterQuery filter = new HisEmteMedicineTypeFilterQuery();
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
