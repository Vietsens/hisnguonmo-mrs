using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTranPatiForm
{
    public partial class HisTranPatiFormDAO : EntityBase
    {
        private HisTranPatiFormGet GetWorker
        {
            get
            {
                return (HisTranPatiFormGet)Worker.Get<HisTranPatiFormGet>();
            }
        }
        public List<HIS_TRAN_PATI_FORM> Get(HisTranPatiFormSO search, CommonParam param)
        {
            List<HIS_TRAN_PATI_FORM> result = new List<HIS_TRAN_PATI_FORM>();
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

        public HIS_TRAN_PATI_FORM GetById(long id, HisTranPatiFormSO search)
        {
            HIS_TRAN_PATI_FORM result = null;
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
