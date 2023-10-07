using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisRehaTrainUnit
{
    public partial class HisRehaTrainUnitDAO : EntityBase
    {
        public HIS_REHA_TRAIN_UNIT GetByCode(string code, HisRehaTrainUnitSO search)
        {
            HIS_REHA_TRAIN_UNIT result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public Dictionary<string, HIS_REHA_TRAIN_UNIT> GetDicByCode(HisRehaTrainUnitSO search, CommonParam param)
        {
            Dictionary<string, HIS_REHA_TRAIN_UNIT> result = new Dictionary<string, HIS_REHA_TRAIN_UNIT>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }
    }
}
