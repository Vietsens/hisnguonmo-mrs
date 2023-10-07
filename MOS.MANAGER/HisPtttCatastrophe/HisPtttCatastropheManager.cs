using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPtttCatastrophe
{
    public partial class HisPtttCatastropheManager : BusinessBase
    {
        public HisPtttCatastropheManager()
            : base()
        {

        }

        public HisPtttCatastropheManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_PTTT_CATASTROPHE> Get(HisPtttCatastropheFilterQuery filter)
        {
             List<HIS_PTTT_CATASTROPHE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_PTTT_CATASTROPHE> resultData = null;
                if (valid)
                {
                    resultData = new HisPtttCatastropheGet(param).Get(filter);
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

        
        public  HIS_PTTT_CATASTROPHE GetById(long data)
        {
             HIS_PTTT_CATASTROPHE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PTTT_CATASTROPHE resultData = null;
                if (valid)
                {
                    resultData = new HisPtttCatastropheGet(param).GetById(data);
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

        
        public  HIS_PTTT_CATASTROPHE GetById(long data, HisPtttCatastropheFilterQuery filter)
        {
             HIS_PTTT_CATASTROPHE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PTTT_CATASTROPHE resultData = null;
                if (valid)
                {
                    resultData = new HisPtttCatastropheGet(param).GetById(data, filter);
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

        
        public  HIS_PTTT_CATASTROPHE GetByCode(string data)
        {
             HIS_PTTT_CATASTROPHE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PTTT_CATASTROPHE resultData = null;
                if (valid)
                {
                    resultData = new HisPtttCatastropheGet(param).GetByCode(data);
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

        
        public  HIS_PTTT_CATASTROPHE GetByCode(string data, HisPtttCatastropheFilterQuery filter)
        {
             HIS_PTTT_CATASTROPHE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PTTT_CATASTROPHE resultData = null;
                if (valid)
                {
                    resultData = new HisPtttCatastropheGet(param).GetByCode(data, filter);
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
