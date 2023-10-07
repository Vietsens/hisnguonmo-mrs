using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineUseForm
{
    class HisMedicineUseFormGet : GetBase
    {
        internal HisMedicineUseFormGet()
            : base()
        {

        }

        internal HisMedicineUseFormGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEDICINE_USE_FORM> Get(HisMedicineUseFormFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineUseFormDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDICINE_USE_FORM GetById(long id)
        {
            try
            {
                return GetById(id, new HisMedicineUseFormFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDICINE_USE_FORM GetById(long id, HisMedicineUseFormFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineUseFormDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDICINE_USE_FORM GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisMedicineUseFormFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDICINE_USE_FORM GetByCode(string code, HisMedicineUseFormFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineUseFormDAO.GetByCode(code, filter.Query());
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
