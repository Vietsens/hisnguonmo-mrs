using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisRehaTrainUnit
{
    public partial class HisRehaTrainUnitDAO : EntityBase
    {
        private HisRehaTrainUnitGet GetWorker
        {
            get
            {
                return (HisRehaTrainUnitGet)Worker.Get<HisRehaTrainUnitGet>();
            }
        }
        public List<HIS_REHA_TRAIN_UNIT> Get(HisRehaTrainUnitSO search, CommonParam param)
        {
            List<HIS_REHA_TRAIN_UNIT> result = new List<HIS_REHA_TRAIN_UNIT>();
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

        public HIS_REHA_TRAIN_UNIT GetById(long id, HisRehaTrainUnitSO search)
        {
            HIS_REHA_TRAIN_UNIT result = null;
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
