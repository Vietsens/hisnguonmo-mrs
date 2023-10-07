using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSuimSetySuin
{
    public partial class HisSuimSetySuinManager : BusinessBase
    {
        public HisSuimSetySuinManager()
            : base()
        {

        }

        public HisSuimSetySuinManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_SUIM_SETY_SUIN> Get(HisSuimSetySuinFilterQuery filter)
        {
             List<HIS_SUIM_SETY_SUIN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SUIM_SETY_SUIN> resultData = null;
                if (valid)
                {
                    resultData = new HisSuimSetySuinGet(param).Get(filter);
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

        
        public  HIS_SUIM_SETY_SUIN GetById(long data)
        {
             HIS_SUIM_SETY_SUIN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_SUIM_SETY_SUIN resultData = null;
                if (valid)
                {
                    resultData = new HisSuimSetySuinGet(param).GetById(data);
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

        
        public  HIS_SUIM_SETY_SUIN GetById(long data, HisSuimSetySuinFilterQuery filter)
        {
             HIS_SUIM_SETY_SUIN result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_SUIM_SETY_SUIN resultData = null;
                if (valid)
                {
                    resultData = new HisSuimSetySuinGet(param).GetById(data, filter);
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

        
        public  List<HIS_SUIM_SETY_SUIN> GetBySuimIndexId(long id)
        {
             List<HIS_SUIM_SETY_SUIN> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(id);
                List<HIS_SUIM_SETY_SUIN> resultData = null;
                if (valid)
                {
                    resultData = new HisSuimSetySuinGet(param).GetBySuimIndexId(id);
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
