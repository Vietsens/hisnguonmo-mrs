using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisRehaTrainType
{
    public partial class HisRehaTrainTypeDAO : EntityBase
    {
        private HisRehaTrainTypeGet GetWorker
        {
            get
            {
                return (HisRehaTrainTypeGet)Worker.Get<HisRehaTrainTypeGet>();
            }
        }
        public List<HIS_REHA_TRAIN_TYPE> Get(HisRehaTrainTypeSO search, CommonParam param)
        {
            List<HIS_REHA_TRAIN_TYPE> result = new List<HIS_REHA_TRAIN_TYPE>();
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

        public HIS_REHA_TRAIN_TYPE GetById(long id, HisRehaTrainTypeSO search)
        {
            HIS_REHA_TRAIN_TYPE result = null;
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
