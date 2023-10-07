using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPaanLiquid
{
    public partial class HisPaanLiquidManager : BusinessBase
    {
        public HisPaanLiquidManager()
            : base()
        {

        }

        public HisPaanLiquidManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_PAAN_LIQUID> Get(HisPaanLiquidFilterQuery filter)
        {
             List<HIS_PAAN_LIQUID> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_PAAN_LIQUID> resultData = null;
                if (valid)
                {
                    resultData = new HisPaanLiquidGet(param).Get(filter);
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

        
        public  HIS_PAAN_LIQUID GetById(long data)
        {
             HIS_PAAN_LIQUID result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PAAN_LIQUID resultData = null;
                if (valid)
                {
                    resultData = new HisPaanLiquidGet(param).GetById(data);
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

        
        public  HIS_PAAN_LIQUID GetById(long data, HisPaanLiquidFilterQuery filter)
        {
             HIS_PAAN_LIQUID result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PAAN_LIQUID resultData = null;
                if (valid)
                {
                    resultData = new HisPaanLiquidGet(param).GetById(data, filter);
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

        
        public  HIS_PAAN_LIQUID GetByCode(string data)
        {
             HIS_PAAN_LIQUID result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PAAN_LIQUID resultData = null;
                if (valid)
                {
                    resultData = new HisPaanLiquidGet(param).GetByCode(data);
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

        
        public  HIS_PAAN_LIQUID GetByCode(string data, HisPaanLiquidFilterQuery filter)
        {
             HIS_PAAN_LIQUID result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PAAN_LIQUID resultData = null;
                if (valid)
                {
                    resultData = new HisPaanLiquidGet(param).GetByCode(data, filter);
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
