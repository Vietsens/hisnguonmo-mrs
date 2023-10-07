using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTextLib
{
    public partial class HisTextLibManager : BusinessBase
    {
        public HisTextLibManager()
            : base()
        {

        }

        public HisTextLibManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_TEXT_LIB> Get(HisTextLibFilterQuery filter)
        {
            List<HIS_TEXT_LIB> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_TEXT_LIB> resultData = null;
                if (valid)
                {
                    resultData = new HisTextLibGet(param).Get(filter);
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

        
        public HIS_TEXT_LIB GetById(long data)
        {
            HIS_TEXT_LIB result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_TEXT_LIB resultData = null;
                if (valid)
                {
                    resultData = new HisTextLibGet(param).GetById(data);
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

        
        public HIS_TEXT_LIB GetById(long data, HisTextLibFilterQuery filter)
        {
            HIS_TEXT_LIB result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_TEXT_LIB resultData = null;
                if (valid)
                {
                    resultData = new HisTextLibGet(param).GetById(data, filter);
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
