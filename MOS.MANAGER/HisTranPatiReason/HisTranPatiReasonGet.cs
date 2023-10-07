using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTranPatiReason
{
    class HisTranPatiReasonGet : GetBase
    {
        internal HisTranPatiReasonGet()
            : base()
        {

        }

        internal HisTranPatiReasonGet(Inventec.Core.CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_TRAN_PATI_REASON> Get(HisTranPatiReasonFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTranPatiReasonDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TRAN_PATI_REASON GetById(long id)
        {
            try
            {
                return GetById(id, new HisTranPatiReasonFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TRAN_PATI_REASON GetById(long id, HisTranPatiReasonFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTranPatiReasonDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TRAN_PATI_REASON GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisTranPatiReasonFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TRAN_PATI_REASON GetByCode(string code, HisTranPatiReasonFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTranPatiReasonDAO.GetByCode(code, filter.Query());
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
