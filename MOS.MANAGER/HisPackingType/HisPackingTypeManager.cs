using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPackingType
{
    public partial class HisPackingTypeManager : BusinessBase
    {
        public HisPackingTypeManager()
            : base()
        {

        }

        public HisPackingTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_PACKING_TYPE> Get(HisPackingTypeFilterQuery filter)
        {
             List<HIS_PACKING_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_PACKING_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisPackingTypeGet(param).Get(filter);
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

        
        public  HIS_PACKING_TYPE GetById(long data)
        {
             HIS_PACKING_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PACKING_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisPackingTypeGet(param).GetById(data);
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

        
        public  HIS_PACKING_TYPE GetById(long data, HisPackingTypeFilterQuery filter)
        {
             HIS_PACKING_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PACKING_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisPackingTypeGet(param).GetById(data, filter);
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

        
        public  HIS_PACKING_TYPE GetByCode(string data)
        {
             HIS_PACKING_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PACKING_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisPackingTypeGet(param).GetByCode(data);
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

        
        public  HIS_PACKING_TYPE GetByCode(string data, HisPackingTypeFilterQuery filter)
        {
             HIS_PACKING_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PACKING_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisPackingTypeGet(param).GetByCode(data, filter);
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
