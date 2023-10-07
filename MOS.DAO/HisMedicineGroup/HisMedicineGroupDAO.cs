using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMedicineGroup
{
    public partial class HisMedicineGroupDAO : EntityBase
    {
        private HisMedicineGroupGet GetWorker
        {
            get
            {
                return (HisMedicineGroupGet)Worker.Get<HisMedicineGroupGet>();
            }
        }

        public List<HIS_MEDICINE_GROUP> Get(HisMedicineGroupSO search, CommonParam param)
        {
            List<HIS_MEDICINE_GROUP> result = new List<HIS_MEDICINE_GROUP>();
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

        public HIS_MEDICINE_GROUP GetById(long id, HisMedicineGroupSO search)
        {
            HIS_MEDICINE_GROUP result = null;
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
