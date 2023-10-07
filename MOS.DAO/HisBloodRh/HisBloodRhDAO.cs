using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBloodRh
{
    public partial class HisBloodRhDAO : EntityBase
    {
        private HisBloodRhGet GetWorker
        {
            get
            {
                return (HisBloodRhGet)Worker.Get<HisBloodRhGet>();
            }
        }
        public List<HIS_BLOOD_RH> Get(HisBloodRhSO search, CommonParam param)
        {
            List<HIS_BLOOD_RH> result = new List<HIS_BLOOD_RH>();
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

        public HIS_BLOOD_RH GetById(long id, HisBloodRhSO search)
        {
            HIS_BLOOD_RH result = null;
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
