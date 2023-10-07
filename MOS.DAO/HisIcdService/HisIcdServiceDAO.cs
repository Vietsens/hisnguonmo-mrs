using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisIcdService
{
    public partial class HisIcdServiceDAO : EntityBase
    {
        private HisIcdServiceGet GetWorker
        {
            get
            {
                return (HisIcdServiceGet)Worker.Get<HisIcdServiceGet>();
            }
        }

        public List<HIS_ICD_SERVICE> Get(HisIcdServiceSO search, CommonParam param)
        {
            List<HIS_ICD_SERVICE> result = new List<HIS_ICD_SERVICE>();
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

        public HIS_ICD_SERVICE GetById(long id, HisIcdServiceSO search)
        {
            HIS_ICD_SERVICE result = null;
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
