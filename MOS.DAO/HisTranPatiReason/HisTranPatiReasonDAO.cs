using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTranPatiReason
{
    public partial class HisTranPatiReasonDAO : EntityBase
    {
        private HisTranPatiReasonGet GetWorker
        {
            get
            {
                return (HisTranPatiReasonGet)Worker.Get<HisTranPatiReasonGet>();
            }
        }
        public List<HIS_TRAN_PATI_REASON> Get(HisTranPatiReasonSO search, CommonParam param)
        {
            List<HIS_TRAN_PATI_REASON> result = new List<HIS_TRAN_PATI_REASON>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_TRAN_PATI_REASON GetById(long id, HisTranPatiReasonSO search)
        {
            HIS_TRAN_PATI_REASON result = null;
            try
            {
                result = GetWorker.GetById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
