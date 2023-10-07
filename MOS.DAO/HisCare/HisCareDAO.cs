using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisCare
{
    public partial class HisCareDAO : EntityBase
    {
        private HisCareGet GetWorker
        {
            get
            {
                return (HisCareGet)Worker.Get<HisCareGet>();
            }
        }
        public List<HIS_CARE> Get(HisCareSO search, CommonParam param)
        {
            List<HIS_CARE> result = new List<HIS_CARE>();
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

        public HIS_CARE GetById(long id, HisCareSO search)
        {
            HIS_CARE result = null;
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
