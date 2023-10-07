using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRestRetrType
{
    public partial class HisRestRetrTypeManager : BusinessBase
    {
        public HisRestRetrTypeManager()
            : base()
        {

        }

        public HisRestRetrTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_REST_RETR_TYPE> Get(HisRestRetrTypeFilterQuery filter)
        {
             List<HIS_REST_RETR_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_REST_RETR_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisRestRetrTypeGet(param).Get(filter);
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

        
        public  HIS_REST_RETR_TYPE GetById(long data)
        {
             HIS_REST_RETR_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_REST_RETR_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisRestRetrTypeGet(param).GetById(data);
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

        
        public  HIS_REST_RETR_TYPE GetById(long data, HisRestRetrTypeFilterQuery filter)
        {
             HIS_REST_RETR_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_REST_RETR_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisRestRetrTypeGet(param).GetById(data, filter);
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

        
        public  List<HIS_REST_RETR_TYPE> GetByRehaTrainTypeId(long id)
        {
             List<HIS_REST_RETR_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(id);
                List<HIS_REST_RETR_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisRestRetrTypeGet(param).GetByRehaTrainTypeId(id);
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

        
        public  List<HIS_REST_RETR_TYPE> GetByRehaServiceTypeId(long id)
        {
             List<HIS_REST_RETR_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(id);
                List<HIS_REST_RETR_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisRestRetrTypeGet(param).GetByRehaServiceTypeId(id);
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
