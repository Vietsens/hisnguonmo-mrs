using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMedicineType
{
    public partial class HisMedicineTypeDAO : EntityBase
    {
        private HisMedicineTypeGet GetWorker
        {
            get
            {
                return (HisMedicineTypeGet)Worker.Get<HisMedicineTypeGet>();
            }
        }
        public List<HIS_MEDICINE_TYPE> Get(HisMedicineTypeSO search, CommonParam param)
        {
            List<HIS_MEDICINE_TYPE> result = new List<HIS_MEDICINE_TYPE>();
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

        public HIS_MEDICINE_TYPE GetById(long id, HisMedicineTypeSO search)
        {
            HIS_MEDICINE_TYPE result = null;
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
