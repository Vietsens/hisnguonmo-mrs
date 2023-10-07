using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineLine
{
    partial class HisMedicineLineGet : BusinessBase
    {
        internal HisMedicineLineGet()
            : base()
        {

        }

        internal HisMedicineLineGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEDICINE_LINE> Get(HisMedicineLineFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineLineDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDICINE_LINE GetById(long id)
        {
            try
            {
                return GetById(id, new HisMedicineLineFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDICINE_LINE GetById(long id, HisMedicineLineFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineLineDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDICINE_LINE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisMedicineLineFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDICINE_LINE GetByCode(string code, HisMedicineLineFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineLineDAO.GetByCode(code, filter.Query());
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
