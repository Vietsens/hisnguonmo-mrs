using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMedicineUseForm
{
    public partial class HisMedicineUseFormDAO : EntityBase
    {
        private HisMedicineUseFormGet GetWorker
        {
            get
            {
                return (HisMedicineUseFormGet)Worker.Get<HisMedicineUseFormGet>();
            }
        }
        public List<HIS_MEDICINE_USE_FORM> Get(HisMedicineUseFormSO search, CommonParam param)
        {
            List<HIS_MEDICINE_USE_FORM> result = new List<HIS_MEDICINE_USE_FORM>();
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

        public HIS_MEDICINE_USE_FORM GetById(long id, HisMedicineUseFormSO search)
        {
            HIS_MEDICINE_USE_FORM result = null;
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
