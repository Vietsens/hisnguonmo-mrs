using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBornPosition
{
    public partial class HisBornPositionManager : BusinessBase
    {
        public HisBornPositionManager()
            : base()
        {

        }

        public HisBornPositionManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_BORN_POSITION> Get(HisBornPositionFilterQuery filter)
        {
             List<HIS_BORN_POSITION> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_BORN_POSITION> resultData = null;
                if (valid)
                {
                    resultData = new HisBornPositionGet(param).Get(filter);
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

        
        public  HIS_BORN_POSITION GetById(long data)
        {
             HIS_BORN_POSITION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BORN_POSITION resultData = null;
                if (valid)
                {
                    resultData = new HisBornPositionGet(param).GetById(data);
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

        
        public  HIS_BORN_POSITION GetById(long data, HisBornPositionFilterQuery filter)
        {
             HIS_BORN_POSITION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BORN_POSITION resultData = null;
                if (valid)
                {
                    resultData = new HisBornPositionGet(param).GetById(data, filter);
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

        
        public  HIS_BORN_POSITION GetByCode(string data)
        {
             HIS_BORN_POSITION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BORN_POSITION resultData = null;
                if (valid)
                {
                    resultData = new HisBornPositionGet(param).GetByCode(data);
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

        
        public  HIS_BORN_POSITION GetByCode(string data, HisBornPositionFilterQuery filter)
        {
             HIS_BORN_POSITION result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BORN_POSITION resultData = null;
                if (valid)
                {
                    resultData = new HisBornPositionGet(param).GetByCode(data, filter);
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
