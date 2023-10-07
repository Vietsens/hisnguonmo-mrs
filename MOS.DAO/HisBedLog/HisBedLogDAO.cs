using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBedLog
{
    public partial class HisBedLogDAO : EntityBase
    {
        private HisBedLogGet GetWorker
        {
            get
            {
                return (HisBedLogGet)Worker.Get<HisBedLogGet>();
            }
        }
        public List<HIS_BED_LOG> Get(HisBedLogSO search, CommonParam param)
        {
            List<HIS_BED_LOG> result = new List<HIS_BED_LOG>();
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

        public HIS_BED_LOG GetById(long id, HisBedLogSO search)
        {
            HIS_BED_LOG result = null;
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
