using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRehaTrainType
{
    public partial class HisRehaTrainTypeManager : BusinessBase
    {
        public HisRehaTrainTypeManager()
            : base()
        {

        }

        public HisRehaTrainTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_REHA_TRAIN_TYPE> Get(HisRehaTrainTypeFilterQuery filter)
        {
             List<HIS_REHA_TRAIN_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_REHA_TRAIN_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisRehaTrainTypeGet(param).Get(filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public  HIS_REHA_TRAIN_TYPE GetById(long data)
        {
             HIS_REHA_TRAIN_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_REHA_TRAIN_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisRehaTrainTypeGet(param).GetById(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public  HIS_REHA_TRAIN_TYPE GetById(long data, HisRehaTrainTypeFilterQuery filter)
        {
             HIS_REHA_TRAIN_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_REHA_TRAIN_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisRehaTrainTypeGet(param).GetById(data, filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public  List<HIS_REHA_TRAIN_TYPE> GetByRehaTrainUnitId(long rehaTrainUnitId)
        {
             List<HIS_REHA_TRAIN_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(rehaTrainUnitId);
                List<HIS_REHA_TRAIN_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisRehaTrainTypeGet(param).GetByRehaTrainUnitId(rehaTrainUnitId);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }
    }
}
