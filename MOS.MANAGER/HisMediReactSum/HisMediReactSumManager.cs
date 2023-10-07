using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediReactSum
{
    public partial class HisMediReactSumManager : BusinessBase
    {
        public HisMediReactSumManager()
            : base()
        {

        }

        public HisMediReactSumManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_MEDI_REACT_SUM> Get(HisMediReactSumFilterQuery filter)
        {
             List<HIS_MEDI_REACT_SUM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEDI_REACT_SUM> resultData = null;
                if (valid)
                {
                    resultData = new HisMediReactSumGet(param).Get(filter);
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

        
        public  HIS_MEDI_REACT_SUM GetById(long data)
        {
             HIS_MEDI_REACT_SUM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDI_REACT_SUM resultData = null;
                if (valid)
                {
                    resultData = new HisMediReactSumGet(param).GetById(data);
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

        
        public  HIS_MEDI_REACT_SUM GetById(long data, HisMediReactSumFilterQuery filter)
        {
             HIS_MEDI_REACT_SUM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEDI_REACT_SUM resultData = null;
                if (valid)
                {
                    resultData = new HisMediReactSumGet(param).GetById(data, filter);
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
