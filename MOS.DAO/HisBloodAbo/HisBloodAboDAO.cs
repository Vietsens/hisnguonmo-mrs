using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBloodAbo
{
    public partial class HisBloodAboDAO : EntityBase
    {
        private HisBloodAboGet GetWorker
        {
            get
            {
                return (HisBloodAboGet)Worker.Get<HisBloodAboGet>();
            }
        }
        public List<HIS_BLOOD_ABO> Get(HisBloodAboSO search, CommonParam param)
        {
            List<HIS_BLOOD_ABO> result = new List<HIS_BLOOD_ABO>();
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

        public HIS_BLOOD_ABO GetById(long id, HisBloodAboSO search)
        {
            HIS_BLOOD_ABO result = null;
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
