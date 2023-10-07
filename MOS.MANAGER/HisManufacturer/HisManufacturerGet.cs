using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisManufacturer
{
    class HisManufacturerGet : GetBase
    {
        internal HisManufacturerGet()
            : base()
        {

        }

        internal HisManufacturerGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MANUFACTURER> Get(HisManufacturerFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisManufacturerDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MANUFACTURER GetById(long id)
        {
            try
            {
                return GetById(id, new HisManufacturerFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MANUFACTURER GetById(long id, HisManufacturerFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisManufacturerDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MANUFACTURER GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisManufacturerFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MANUFACTURER GetByCode(string code, HisManufacturerFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisManufacturerDAO.GetByCode(code, filter.Query());
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
