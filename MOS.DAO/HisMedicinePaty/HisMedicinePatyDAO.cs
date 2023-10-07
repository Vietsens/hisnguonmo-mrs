using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMedicinePaty
{
    public partial class HisMedicinePatyDAO : EntityBase
    {
        private HisMedicinePatyGet GetWorker
        {
            get
            {
                return (HisMedicinePatyGet)Worker.Get<HisMedicinePatyGet>();
            }
        }
        public List<HIS_MEDICINE_PATY> Get(HisMedicinePatySO search, CommonParam param)
        {
            List<HIS_MEDICINE_PATY> result = new List<HIS_MEDICINE_PATY>();
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

        public HIS_MEDICINE_PATY GetById(long id, HisMedicinePatySO search)
        {
            HIS_MEDICINE_PATY result = null;
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
