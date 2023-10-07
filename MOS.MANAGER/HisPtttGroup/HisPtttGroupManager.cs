using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPtttGroup
{
    public partial class HisPtttGroupManager : BusinessBase
    {
        public HisPtttGroupManager()
            : base()
        {

        }

        public HisPtttGroupManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_PTTT_GROUP> Get(HisPtttGroupFilterQuery filter)
        {
             List<HIS_PTTT_GROUP> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_PTTT_GROUP> resultData = null;
                if (valid)
                {
                    resultData = new HisPtttGroupGet(param).Get(filter);
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

        
        public  HIS_PTTT_GROUP GetById(long data)
        {
             HIS_PTTT_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PTTT_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisPtttGroupGet(param).GetById(data);
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

        
        public  HIS_PTTT_GROUP GetById(long data, HisPtttGroupFilterQuery filter)
        {
             HIS_PTTT_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PTTT_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisPtttGroupGet(param).GetById(data, filter);
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

        
        public  HIS_PTTT_GROUP GetByCode(string data)
        {
             HIS_PTTT_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PTTT_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisPtttGroupGet(param).GetByCode(data);
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

        
        public  HIS_PTTT_GROUP GetByCode(string data, HisPtttGroupFilterQuery filter)
        {
             HIS_PTTT_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PTTT_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisPtttGroupGet(param).GetByCode(data, filter);
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
