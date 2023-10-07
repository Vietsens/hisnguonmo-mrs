using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBaby
{
    public partial class HisBabyDAO : EntityBase
    {
        private HisBabyGet GetWorker
        {
            get
            {
                return (HisBabyGet)Worker.Get<HisBabyGet>();
            }
        }
        public List<HIS_BABY> Get(HisBabySO search, CommonParam param)
        {
            List<HIS_BABY> result = new List<HIS_BABY>();
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

        public HIS_BABY GetById(long id, HisBabySO search)
        {
            HIS_BABY result = null;
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
