using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMedicine
{
    public partial class HisMedicineDAO : EntityBase
    {
        private HisMedicineGet GetWorker
        {
            get
            {
                return (HisMedicineGet)Worker.Get<HisMedicineGet>();
            }
        }
        public List<HIS_MEDICINE> Get(HisMedicineSO search, CommonParam param)
        {
            List<HIS_MEDICINE> result = new List<HIS_MEDICINE>();
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

        public HIS_MEDICINE GetById(long id, HisMedicineSO search)
        {
            HIS_MEDICINE result = null;
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
