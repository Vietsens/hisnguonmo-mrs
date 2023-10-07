using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBloodGroup
{
    public partial class HisBloodGroupDAO : EntityBase
    {
        private HisBloodGroupGet GetWorker
        {
            get
            {
                return (HisBloodGroupGet)Worker.Get<HisBloodGroupGet>();
            }
        }
        public List<HIS_BLOOD_GROUP> Get(HisBloodGroupSO search, CommonParam param)
        {
            List<HIS_BLOOD_GROUP> result = new List<HIS_BLOOD_GROUP>();
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

        public HIS_BLOOD_GROUP GetById(long id, HisBloodGroupSO search)
        {
            HIS_BLOOD_GROUP result = null;
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
