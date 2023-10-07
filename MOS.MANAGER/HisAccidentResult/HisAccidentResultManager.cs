using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentResult
{
    public partial class HisAccidentResultManager : BusinessBase
    {
        public HisAccidentResultManager()
            : base()
        {

        }

        public HisAccidentResultManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_ACCIDENT_RESULT> Get(HisAccidentResultFilterQuery filter)
        {
             List<HIS_ACCIDENT_RESULT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_ACCIDENT_RESULT> resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentResultGet(param).Get(filter);
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

        
        public  HIS_ACCIDENT_RESULT GetById(long data)
        {
             HIS_ACCIDENT_RESULT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ACCIDENT_RESULT resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentResultGet(param).GetById(data);
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

        
        public  HIS_ACCIDENT_RESULT GetById(long data, HisAccidentResultFilterQuery filter)
        {
             HIS_ACCIDENT_RESULT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ACCIDENT_RESULT resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentResultGet(param).GetById(data, filter);
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

        
        public  HIS_ACCIDENT_RESULT GetByCode(string data)
        {
             HIS_ACCIDENT_RESULT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ACCIDENT_RESULT resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentResultGet(param).GetByCode(data);
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

        
        public  HIS_ACCIDENT_RESULT GetByCode(string data, HisAccidentResultFilterQuery filter)
        {
             HIS_ACCIDENT_RESULT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ACCIDENT_RESULT resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentResultGet(param).GetByCode(data, filter);
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
