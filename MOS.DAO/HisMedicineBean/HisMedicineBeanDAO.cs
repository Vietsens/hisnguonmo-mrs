using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMedicineBean
{
    public partial class HisMedicineBeanDAO : EntityBase
    {
        private HisMedicineBeanGet GetWorker
        {
            get
            {
                return (HisMedicineBeanGet)Worker.Get<HisMedicineBeanGet>();
            }
        }
        public List<HIS_MEDICINE_BEAN> Get(HisMedicineBeanSO search, CommonParam param)
        {
            List<HIS_MEDICINE_BEAN> result = new List<HIS_MEDICINE_BEAN>();
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

        public HIS_MEDICINE_BEAN GetById(long id, HisMedicineBeanSO search)
        {
            HIS_MEDICINE_BEAN result = null;
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
