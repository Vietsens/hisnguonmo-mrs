using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediReactType
{
    public partial class HisMediReactTypeManager : BusinessBase
    {
        public HisMediReactTypeManager()
            : base()
        {

        }

        public HisMediReactTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_MEDI_REACT_TYPE> Get(HisMediReactTypeFilterQuery filter)
        {
             List<HIS_MEDI_REACT_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEDI_REACT_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisMediReactTypeGet(param).Get(filter);
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

        
        public  HIS_MEDI_REACT_TYPE GetById(long data)
        {
             HIS_MEDI_REACT_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDI_REACT_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMediReactTypeGet(param).GetById(data);
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

        
        public  HIS_MEDI_REACT_TYPE GetById(long data, HisMediReactTypeFilterQuery filter)
        {
             HIS_MEDI_REACT_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEDI_REACT_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMediReactTypeGet(param).GetById(data, filter);
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

        
        public  HIS_MEDI_REACT_TYPE GetByCode(string data)
        {
             HIS_MEDI_REACT_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDI_REACT_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMediReactTypeGet(param).GetByCode(data);
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

        
        public  HIS_MEDI_REACT_TYPE GetByCode(string data, HisMediReactTypeFilterQuery filter)
        {
             HIS_MEDI_REACT_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEDI_REACT_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMediReactTypeGet(param).GetByCode(data, filter);
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
