using Inventec.Common.Repository;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccidentCare
{
    public partial class HisAccidentCareDAO : EntityBase
    {
        private HisAccidentCareGet GetWorker
        {
            get
            {
                return (HisAccidentCareGet)Worker.Get<HisAccidentCareGet>();
            }
        }
        public List<HIS_ACCIDENT_CARE> Get(HisAccidentCareSO search, CommonParam param)
        {
            List<HIS_ACCIDENT_CARE> result = new List<HIS_ACCIDENT_CARE>();
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

        public HIS_ACCIDENT_CARE GetById(long id, HisAccidentCareSO search)
        {
            HIS_ACCIDENT_CARE result = null;
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
