using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMedicineTypeAcin
{
    public partial class HisMedicineTypeAcinDAO : EntityBase
    {
        private HisMedicineTypeAcinGet GetWorker
        {
            get
            {
                return (HisMedicineTypeAcinGet)Worker.Get<HisMedicineTypeAcinGet>();
            }
        }
        public List<HIS_MEDICINE_TYPE_ACIN> Get(HisMedicineTypeAcinSO search, CommonParam param)
        {
            List<HIS_MEDICINE_TYPE_ACIN> result = new List<HIS_MEDICINE_TYPE_ACIN>();
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

        public HIS_MEDICINE_TYPE_ACIN GetById(long id, HisMedicineTypeAcinSO search)
        {
            HIS_MEDICINE_TYPE_ACIN result = null;
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
