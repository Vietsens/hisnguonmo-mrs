using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisIcdGroup
{
    public partial class HisIcdGroupDAO : EntityBase
    {
        private HisIcdGroupGet GetWorker
        {
            get
            {
                return (HisIcdGroupGet)Worker.Get<HisIcdGroupGet>();
            }
        }
        public List<HIS_ICD_GROUP> Get(HisIcdGroupSO search, CommonParam param)
        {
            List<HIS_ICD_GROUP> result = new List<HIS_ICD_GROUP>();
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

        public HIS_ICD_GROUP GetById(long id, HisIcdGroupSO search)
        {
            HIS_ICD_GROUP result = null;
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
