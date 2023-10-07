using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisIcdCm
{
    public partial class HisIcdCmDAO : EntityBase
    {
        private HisIcdCmGet GetWorker
        {
            get
            {
                return (HisIcdCmGet)Worker.Get<HisIcdCmGet>();
            }
        }
        public List<HIS_ICD_CM> Get(HisIcdCmSO search, CommonParam param)
        {
            List<HIS_ICD_CM> result = new List<HIS_ICD_CM>();
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

        public HIS_ICD_CM GetById(long id, HisIcdCmSO search)
        {
            HIS_ICD_CM result = null;
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
