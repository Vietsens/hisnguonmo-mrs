using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPtttTable
{
    public partial class HisPtttTableManager : BusinessBase
    {
        public HisPtttTableManager()
            : base()
        {

        }

        public HisPtttTableManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_PTTT_TABLE> Get(HisPtttTableFilterQuery filter)
        {
             List<HIS_PTTT_TABLE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_PTTT_TABLE> resultData = null;
                if (valid)
                {
                    resultData = new HisPtttTableGet(param).Get(filter);
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

        
        public  HIS_PTTT_TABLE GetById(long data)
        {
             HIS_PTTT_TABLE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PTTT_TABLE resultData = null;
                if (valid)
                {
                    resultData = new HisPtttTableGet(param).GetById(data);
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

        
        public  HIS_PTTT_TABLE GetById(long data, HisPtttTableFilterQuery filter)
        {
             HIS_PTTT_TABLE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PTTT_TABLE resultData = null;
                if (valid)
                {
                    resultData = new HisPtttTableGet(param).GetById(data, filter);
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

        
        public  HIS_PTTT_TABLE GetByCode(string data)
        {
             HIS_PTTT_TABLE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PTTT_TABLE resultData = null;
                if (valid)
                {
                    resultData = new HisPtttTableGet(param).GetByCode(data);
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

        
        public  HIS_PTTT_TABLE GetByCode(string data, HisPtttTableFilterQuery filter)
        {
             HIS_PTTT_TABLE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PTTT_TABLE resultData = null;
                if (valid)
                {
                    resultData = new HisPtttTableGet(param).GetByCode(data, filter);
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
