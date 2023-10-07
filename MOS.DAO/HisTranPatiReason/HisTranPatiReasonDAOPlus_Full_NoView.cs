using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTranPatiReason
{
    public partial class HisTranPatiReasonDAO : EntityBase
    {
        public HIS_TRAN_PATI_REASON GetByCode(string code, HisTranPatiReasonSO search)
        {
            HIS_TRAN_PATI_REASON result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public Dictionary<string, HIS_TRAN_PATI_REASON> GetDicByCode(HisTranPatiReasonSO search, CommonParam param)
        {
            Dictionary<string, HIS_TRAN_PATI_REASON> result = new Dictionary<string, HIS_TRAN_PATI_REASON>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }
    }
}
