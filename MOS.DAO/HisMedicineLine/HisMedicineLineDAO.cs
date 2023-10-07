using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMedicineLine
{
    public partial class HisMedicineLineDAO : EntityBase
    {
        private HisMedicineLineGet GetWorker
        {
            get
            {
                return (HisMedicineLineGet)Worker.Get<HisMedicineLineGet>();
            }
        }
        public List<HIS_MEDICINE_LINE> Get(HisMedicineLineSO search, CommonParam param)
        {
            List<HIS_MEDICINE_LINE> result = new List<HIS_MEDICINE_LINE>();
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

        public HIS_MEDICINE_LINE GetById(long id, HisMedicineLineSO search)
        {
            HIS_MEDICINE_LINE result = null;
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
