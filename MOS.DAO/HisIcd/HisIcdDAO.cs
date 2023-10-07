using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisIcd
{
    public partial class HisIcdDAO : EntityBase
    {
        private HisIcdGet GetWorker
        {
            get
            {
                return (HisIcdGet)Worker.Get<HisIcdGet>();
            }
        }
        public List<HIS_ICD> Get(HisIcdSO search, CommonParam param)
        {
            List<HIS_ICD> result = new List<HIS_ICD>();
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

        public HIS_ICD GetById(long id, HisIcdSO search)
        {
            HIS_ICD result = null;
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
