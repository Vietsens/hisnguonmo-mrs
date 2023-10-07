using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBidMaterialType
{
    public partial class HisBidMaterialTypeManager : BusinessBase
    {
        public HisBidMaterialTypeManager()
            : base()
        {

        }

        public HisBidMaterialTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_BID_MATERIAL_TYPE> Get(HisBidMaterialTypeFilterQuery filter)
        {
             List<HIS_BID_MATERIAL_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_BID_MATERIAL_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisBidMaterialTypeGet(param).Get(filter);
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

        
        public  HIS_BID_MATERIAL_TYPE GetById(long data)
        {
             HIS_BID_MATERIAL_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BID_MATERIAL_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBidMaterialTypeGet(param).GetById(data);
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

        
        public  HIS_BID_MATERIAL_TYPE GetById(long data, HisBidMaterialTypeFilterQuery filter)
        {
             HIS_BID_MATERIAL_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BID_MATERIAL_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisBidMaterialTypeGet(param).GetById(data, filter);
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
