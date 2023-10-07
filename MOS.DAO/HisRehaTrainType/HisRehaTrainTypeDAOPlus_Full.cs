using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisRehaTrainType
{
    public partial class HisRehaTrainTypeDAO : EntityBase
    {
        public List<V_HIS_REHA_TRAIN_TYPE> GetView(HisRehaTrainTypeSO search, CommonParam param)
        {
            List<V_HIS_REHA_TRAIN_TYPE> result = new List<V_HIS_REHA_TRAIN_TYPE>();

            try
            {
                result = GetWorker.GetView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }

        public HIS_REHA_TRAIN_TYPE GetByCode(string code, HisRehaTrainTypeSO search)
        {
            HIS_REHA_TRAIN_TYPE result = null;

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
        
        public V_HIS_REHA_TRAIN_TYPE GetViewById(long id, HisRehaTrainTypeSO search)
        {
            V_HIS_REHA_TRAIN_TYPE result = null;

            try
            {
                result = GetWorker.GetViewById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public V_HIS_REHA_TRAIN_TYPE GetViewByCode(string code, HisRehaTrainTypeSO search)
        {
            V_HIS_REHA_TRAIN_TYPE result = null;

            try
            {
                result = GetWorker.GetViewByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public Dictionary<string, HIS_REHA_TRAIN_TYPE> GetDicByCode(HisRehaTrainTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_REHA_TRAIN_TYPE> result = new Dictionary<string, HIS_REHA_TRAIN_TYPE>();
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
