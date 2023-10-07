using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBidMedicineType
{
    public partial class HisBidMedicineTypeDAO : EntityBase
    {
        private HisBidMedicineTypeGet GetWorker
        {
            get
            {
                return (HisBidMedicineTypeGet)Worker.Get<HisBidMedicineTypeGet>();
            }
        }
        public List<HIS_BID_MEDICINE_TYPE> Get(HisBidMedicineTypeSO search, CommonParam param)
        {
            List<HIS_BID_MEDICINE_TYPE> result = new List<HIS_BID_MEDICINE_TYPE>();
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

        public HIS_BID_MEDICINE_TYPE GetById(long id, HisBidMedicineTypeSO search)
        {
            HIS_BID_MEDICINE_TYPE result = null;
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
