using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBornResult
{
    public partial class HisBornResultManager : BusinessBase
    {
        public HisBornResultManager()
            : base()
        {

        }

        public HisBornResultManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_BORN_RESULT> Get(HisBornResultFilterQuery filter)
        {
             List<HIS_BORN_RESULT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_BORN_RESULT> resultData = null;
                if (valid)
                {
                    resultData = new HisBornResultGet(param).Get(filter);
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

        
        public  HIS_BORN_RESULT GetById(long data)
        {
             HIS_BORN_RESULT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BORN_RESULT resultData = null;
                if (valid)
                {
                    resultData = new HisBornResultGet(param).GetById(data);
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

        
        public  HIS_BORN_RESULT GetById(long data, HisBornResultFilterQuery filter)
        {
             HIS_BORN_RESULT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BORN_RESULT resultData = null;
                if (valid)
                {
                    resultData = new HisBornResultGet(param).GetById(data, filter);
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

        
        public  HIS_BORN_RESULT GetByCode(string data)
        {
             HIS_BORN_RESULT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BORN_RESULT resultData = null;
                if (valid)
                {
                    resultData = new HisBornResultGet(param).GetByCode(data);
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

        
        public  HIS_BORN_RESULT GetByCode(string data, HisBornResultFilterQuery filter)
        {
             HIS_BORN_RESULT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BORN_RESULT resultData = null;
                if (valid)
                {
                    resultData = new HisBornResultGet(param).GetByCode(data, filter);
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
