using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTranPatiForm
{
    public partial class HisTranPatiFormDAO : EntityBase
    {
        public HIS_TRAN_PATI_FORM GetByCode(string code, HisTranPatiFormSO search)
        {
            HIS_TRAN_PATI_FORM result = null;

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

        public Dictionary<string, HIS_TRAN_PATI_FORM> GetDicByCode(HisTranPatiFormSO search, CommonParam param)
        {
            Dictionary<string, HIS_TRAN_PATI_FORM> result = new Dictionary<string, HIS_TRAN_PATI_FORM>();
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
