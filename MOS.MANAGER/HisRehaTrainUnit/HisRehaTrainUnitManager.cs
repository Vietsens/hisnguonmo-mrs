using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRehaTrainUnit
{
    public partial class HisRehaTrainUnitManager : BusinessBase
    {
        public HisRehaTrainUnitManager()
            : base()
        {

        }

        public HisRehaTrainUnitManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_REHA_TRAIN_UNIT> Get(HisRehaTrainUnitFilterQuery filter)
        {
             List<HIS_REHA_TRAIN_UNIT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_REHA_TRAIN_UNIT> resultData = null;
                if (valid)
                {
                    resultData = new HisRehaTrainUnitGet(param).Get(filter);
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

        
        public  HIS_REHA_TRAIN_UNIT GetById(long data)
        {
             HIS_REHA_TRAIN_UNIT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_REHA_TRAIN_UNIT resultData = null;
                if (valid)
                {
                    resultData = new HisRehaTrainUnitGet(param).GetById(data);
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

        
        public  HIS_REHA_TRAIN_UNIT GetById(long data, HisRehaTrainUnitFilterQuery filter)
        {
             HIS_REHA_TRAIN_UNIT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_REHA_TRAIN_UNIT resultData = null;
                if (valid)
                {
                    resultData = new HisRehaTrainUnitGet(param).GetById(data, filter);
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

        
        public  HIS_REHA_TRAIN_UNIT GetByCode(string data)
        {
             HIS_REHA_TRAIN_UNIT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_REHA_TRAIN_UNIT resultData = null;
                if (valid)
                {
                    resultData = new HisRehaTrainUnitGet(param).GetByCode(data);
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

        
        public  HIS_REHA_TRAIN_UNIT GetByCode(string data, HisRehaTrainUnitFilterQuery filter)
        {
             HIS_REHA_TRAIN_UNIT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_REHA_TRAIN_UNIT resultData = null;
                if (valid)
                {
                    resultData = new HisRehaTrainUnitGet(param).GetByCode(data, filter);
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
