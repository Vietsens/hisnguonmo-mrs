using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentPoison
{
    public partial class HisAccidentPoisonManager : BusinessBase
    {
        public HisAccidentPoisonManager()
            : base()
        {

        }

        public HisAccidentPoisonManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_ACCIDENT_POISON> Get(HisAccidentPoisonFilterQuery filter)
        {
             List<HIS_ACCIDENT_POISON> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_ACCIDENT_POISON> resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentPoisonGet(param).Get(filter);
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

        
        public  HIS_ACCIDENT_POISON GetById(long data)
        {
             HIS_ACCIDENT_POISON result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ACCIDENT_POISON resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentPoisonGet(param).GetById(data);
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

        
        public  HIS_ACCIDENT_POISON GetById(long data, HisAccidentPoisonFilterQuery filter)
        {
             HIS_ACCIDENT_POISON result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ACCIDENT_POISON resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentPoisonGet(param).GetById(data, filter);
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

        
        public  HIS_ACCIDENT_POISON GetByCode(string data)
        {
             HIS_ACCIDENT_POISON result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ACCIDENT_POISON resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentPoisonGet(param).GetByCode(data);
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

        
        public  HIS_ACCIDENT_POISON GetByCode(string data, HisAccidentPoisonFilterQuery filter)
        {
             HIS_ACCIDENT_POISON result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ACCIDENT_POISON resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentPoisonGet(param).GetByCode(data, filter);
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
