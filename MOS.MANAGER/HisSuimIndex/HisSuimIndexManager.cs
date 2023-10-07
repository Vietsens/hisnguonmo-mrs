using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSuimIndex
{
    public partial class HisSuimIndexManager : BusinessBase
    {
        public HisSuimIndexManager()
            : base()
        {

        }

        public HisSuimIndexManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_SUIM_INDEX> Get(HisSuimIndexFilterQuery filter)
        {
            List<HIS_SUIM_INDEX> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SUIM_INDEX> resultData = null;
                if (valid)
                {
                    resultData = new HisSuimIndexGet(param).Get(filter);
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

        
        public HIS_SUIM_INDEX GetById(long data)
        {
            HIS_SUIM_INDEX result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SUIM_INDEX resultData = null;
                if (valid)
                {
                    resultData = new HisSuimIndexGet(param).GetById(data);
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

        
        public HIS_SUIM_INDEX GetById(long data, HisSuimIndexFilterQuery filter)
        {
            HIS_SUIM_INDEX result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SUIM_INDEX resultData = null;
                if (valid)
                {
                    resultData = new HisSuimIndexGet(param).GetById(data, filter);
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

        
        public HIS_SUIM_INDEX GetByCode(string data)
        {
            HIS_SUIM_INDEX result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SUIM_INDEX resultData = null;
                if (valid)
                {
                    resultData = new HisSuimIndexGet(param).GetByCode(data);
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

        
        public HIS_SUIM_INDEX GetByCode(string data, HisSuimIndexFilterQuery filter)
        {
            HIS_SUIM_INDEX result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SUIM_INDEX resultData = null;
                if (valid)
                {
                    resultData = new HisSuimIndexGet(param).GetByCode(data, filter);
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

        
        public List<HIS_SUIM_INDEX> GetBySuimIndexUnitId(long data)
        {
            List<HIS_SUIM_INDEX> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_SUIM_INDEX> resultData = null;
                if (valid)
                {
                    resultData = new HisSuimIndexGet(param).GetBySuimIndexUnitId(data);
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
