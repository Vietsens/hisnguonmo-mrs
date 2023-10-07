using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisProgram
{
    public partial class HisProgramManager : BusinessBase
    {
        public HisProgramManager()
            : base()
        {

        }

        public HisProgramManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_PROGRAM> Get(HisProgramFilterQuery filter)
        {
            List<HIS_PROGRAM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_PROGRAM> resultData = null;
                if (valid)
                {
                    resultData = new HisProgramGet(param).Get(filter);
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

        
        public HIS_PROGRAM GetById(long data)
        {
            HIS_PROGRAM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PROGRAM resultData = null;
                if (valid)
                {
                    resultData = new HisProgramGet(param).GetById(data);
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

        
        public HIS_PROGRAM GetById(long data, HisProgramFilterQuery filter)
        {
            HIS_PROGRAM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PROGRAM resultData = null;
                if (valid)
                {
                    resultData = new HisProgramGet(param).GetById(data, filter);
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

        
        public HIS_PROGRAM GetByCode(string data)
        {
            HIS_PROGRAM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PROGRAM resultData = null;
                if (valid)
                {
                    resultData = new HisProgramGet(param).GetByCode(data);
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

        
        public HIS_PROGRAM GetByCode(string data, HisProgramFilterQuery filter)
        {
            HIS_PROGRAM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PROGRAM resultData = null;
                if (valid)
                {
                    resultData = new HisProgramGet(param).GetByCode(data, filter);
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
