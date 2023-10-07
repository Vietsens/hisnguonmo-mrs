using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSuimIndexUnit
{
    public partial class HisSuimIndexUnitManager : BusinessBase
    {
        public HisSuimIndexUnitManager()
            : base()
        {

        }

        public HisSuimIndexUnitManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_SUIM_INDEX_UNIT> Get(HisSuimIndexUnitFilterQuery filter)
        {
             List<HIS_SUIM_INDEX_UNIT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SUIM_INDEX_UNIT> resultData = null;
                if (valid)
                {
                    resultData = new HisSuimIndexUnitGet(param).Get(filter);
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

        
        public  HIS_SUIM_INDEX_UNIT GetById(long data)
        {
             HIS_SUIM_INDEX_UNIT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SUIM_INDEX_UNIT resultData = null;
                if (valid)
                {
                    resultData = new HisSuimIndexUnitGet(param).GetById(data);
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

        
        public  HIS_SUIM_INDEX_UNIT GetById(long data, HisSuimIndexUnitFilterQuery filter)
        {
             HIS_SUIM_INDEX_UNIT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SUIM_INDEX_UNIT resultData = null;
                if (valid)
                {
                    resultData = new HisSuimIndexUnitGet(param).GetById(data, filter);
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

        
        public  HIS_SUIM_INDEX_UNIT GetByCode(string data)
        {
             HIS_SUIM_INDEX_UNIT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SUIM_INDEX_UNIT resultData = null;
                if (valid)
                {
                    resultData = new HisSuimIndexUnitGet(param).GetByCode(data);
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

        
        public  HIS_SUIM_INDEX_UNIT GetByCode(string data, HisSuimIndexUnitFilterQuery filter)
        {
             HIS_SUIM_INDEX_UNIT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SUIM_INDEX_UNIT resultData = null;
                if (valid)
                {
                    resultData = new HisSuimIndexUnitGet(param).GetByCode(data, filter);
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
