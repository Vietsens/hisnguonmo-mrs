using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineTypeAcin
{
    class HisMedicineTypeAcinGet : GetBase
    {
        internal HisMedicineTypeAcinGet()
            : base()
        {

        }

        internal HisMedicineTypeAcinGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEDICINE_TYPE_ACIN> Get(HisMedicineTypeAcinFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineTypeAcinDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MEDICINE_TYPE_ACIN> GetView(HisMedicineTypeAcinViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineTypeAcinDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDICINE_TYPE_ACIN GetById(long id)
        {
            try
            {
                return GetById(id, new HisMedicineTypeAcinFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDICINE_TYPE_ACIN GetById(long id, HisMedicineTypeAcinFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineTypeAcinDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_MEDICINE_TYPE_ACIN GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisMedicineTypeAcinViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEDICINE_TYPE_ACIN GetViewById(long id, HisMedicineTypeAcinViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineTypeAcinDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEDICINE_TYPE_ACIN> GetByActiveIngredientId(long activeIngredientId)
        {
            try
            {
                HisMedicineTypeAcinFilterQuery filter = new HisMedicineTypeAcinFilterQuery();
                filter.ACTIVE_INGREDIENT_ID = activeIngredientId;
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
