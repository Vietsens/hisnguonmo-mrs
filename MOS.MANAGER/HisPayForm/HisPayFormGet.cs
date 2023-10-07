using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPayForm
{
    class HisPayFormGet : GetBase
    {
        internal HisPayFormGet()
            : base()
        {

        }

        internal HisPayFormGet(Inventec.Core.CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_PAY_FORM> Get(HisPayFormFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPayFormDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PAY_FORM GetById(long id)
        {
            try
            {
                return GetById(id, new HisPayFormFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PAY_FORM GetById(long id, HisPayFormFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPayFormDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PAY_FORM GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisPayFormFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PAY_FORM GetByCode(string code, HisPayFormFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPayFormDAO.GetByCode(code, filter.Query());
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
