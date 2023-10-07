using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestMetyDepa
{
    public partial class HisMestMetyDepaManager : BusinessBase
    {
        public HisMestMetyDepaManager()
            : base()
        {

        }

        public HisMestMetyDepaManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_MEST_METY_DEPA> Get(HisMestMetyDepaFilterQuery filter)
        {
             List<HIS_MEST_METY_DEPA> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEST_METY_DEPA> resultData = null;
                if (valid)
                {
                    resultData = new HisMestMetyDepaGet(param).Get(filter);
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

        
        public  HIS_MEST_METY_DEPA GetById(long data)
        {
             HIS_MEST_METY_DEPA result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEST_METY_DEPA resultData = null;
                if (valid)
                {
                    resultData = new HisMestMetyDepaGet(param).GetById(data);
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

        
        public  HIS_MEST_METY_DEPA GetById(long data, HisMestMetyDepaFilterQuery filter)
        {
             HIS_MEST_METY_DEPA result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEST_METY_DEPA resultData = null;
                if (valid)
                {
                    resultData = new HisMestMetyDepaGet(param).GetById(data, filter);
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
