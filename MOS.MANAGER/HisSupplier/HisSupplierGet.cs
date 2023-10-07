using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSupplier
{
    class HisSupplierGet : GetBase
    {
        internal HisSupplierGet()
            : base()
        {

        }

        internal HisSupplierGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SUPPLIER> Get(HisSupplierFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSupplierDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SUPPLIER GetById(long id)
        {
            try
            {
                return GetById(id, new HisSupplierFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SUPPLIER GetById(long id, HisSupplierFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSupplierDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SUPPLIER GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisSupplierFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SUPPLIER GetByCode(string code, HisSupplierFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSupplierDAO.GetByCode(code, filter.Query());
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
