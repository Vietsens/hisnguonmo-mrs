using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTranPatiForm
{
    class HisTranPatiFormGet : GetBase
    {
        internal HisTranPatiFormGet()
            : base()
        {

        }

        internal HisTranPatiFormGet(Inventec.Core.CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_TRAN_PATI_FORM> Get(HisTranPatiFormFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTranPatiFormDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TRAN_PATI_FORM GetById(long id)
        {
            try
            {
                return GetById(id, new HisTranPatiFormFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TRAN_PATI_FORM GetById(long id, HisTranPatiFormFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTranPatiFormDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TRAN_PATI_FORM GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisTranPatiFormFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TRAN_PATI_FORM GetByCode(string code, HisTranPatiFormFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTranPatiFormDAO.GetByCode(code, filter.Query());
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
