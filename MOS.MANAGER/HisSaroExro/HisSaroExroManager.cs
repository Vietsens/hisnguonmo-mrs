using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSaroExro
{
    public partial class HisSaroExroManager : BusinessBase
    {
        public HisSaroExroManager()
            : base()
        {

        }
        
        public HisSaroExroManager(CommonParam param)
            : base(param)
        {

        }
		
		
        public  List<HIS_SARO_EXRO> Get(HisSaroExroFilterQuery filter)
        {
             List<HIS_SARO_EXRO> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_SARO_EXRO> resultData = null;
                if (valid)
                {
                    resultData = new HisSaroExroGet(param).Get(filter);
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

        
        public  List<V_HIS_SARO_EXRO> GetView(HisSaroExroViewFilterQuery filter)
        {
             List<V_HIS_SARO_EXRO> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SARO_EXRO> resultData = null;
                if (valid)
                {
                    resultData = new HisSaroExroGet(param).GetView(filter);
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
