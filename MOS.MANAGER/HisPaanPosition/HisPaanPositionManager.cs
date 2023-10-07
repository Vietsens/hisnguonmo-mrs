using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPaanPosition
{
    public partial class HisPaanPositionManager : BusinessBase
    {
        public HisPaanPositionManager()
            : base()
        {

        }

        public HisPaanPositionManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_PAAN_POSITION> Get(HisPaanPositionFilterQuery filter)
        {
             List<HIS_PAAN_POSITION> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_PAAN_POSITION> resultData = null;
                if (valid)
                {
                    resultData = new HisPaanPositionGet(param).Get(filter);
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

        
        public  HIS_PAAN_POSITION GetById(long data)
        {
             HIS_PAAN_POSITION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PAAN_POSITION resultData = null;
                if (valid)
                {
                    resultData = new HisPaanPositionGet(param).GetById(data);
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

        
        public  HIS_PAAN_POSITION GetById(long data, HisPaanPositionFilterQuery filter)
        {
             HIS_PAAN_POSITION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PAAN_POSITION resultData = null;
                if (valid)
                {
                    resultData = new HisPaanPositionGet(param).GetById(data, filter);
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

        
        public  HIS_PAAN_POSITION GetByCode(string data)
        {
             HIS_PAAN_POSITION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PAAN_POSITION resultData = null;
                if (valid)
                {
                    resultData = new HisPaanPositionGet(param).GetByCode(data);
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

        
        public  HIS_PAAN_POSITION GetByCode(string data, HisPaanPositionFilterQuery filter)
        {
             HIS_PAAN_POSITION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PAAN_POSITION resultData = null;
                if (valid)
                {
                    resultData = new HisPaanPositionGet(param).GetByCode(data, filter);
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
