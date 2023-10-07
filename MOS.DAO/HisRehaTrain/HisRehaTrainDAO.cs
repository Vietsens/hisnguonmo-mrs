using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisRehaTrain
{
    public partial class HisRehaTrainDAO : EntityBase
    {
        private HisRehaTrainGet GetWorker
        {
            get
            {
                return (HisRehaTrainGet)Worker.Get<HisRehaTrainGet>();
            }
        }
        public List<HIS_REHA_TRAIN> Get(HisRehaTrainSO search, CommonParam param)
        {
            List<HIS_REHA_TRAIN> result = new List<HIS_REHA_TRAIN>();
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

        public HIS_REHA_TRAIN GetById(long id, HisRehaTrainSO search)
        {
            HIS_REHA_TRAIN result = null;
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
