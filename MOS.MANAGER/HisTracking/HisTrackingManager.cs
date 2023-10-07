using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTracking
{
    public partial class HisTrackingManager : BusinessBase
    {
        public HisTrackingManager()
            : base()
        {

        }

        public HisTrackingManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_TRACKING> Get(HisTrackingFilterQuery filter)
        {
             List<HIS_TRACKING> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_TRACKING> resultData = null;
                if (valid)
                {
                    resultData = new HisTrackingGet(param).Get(filter);
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

        
        public  HIS_TRACKING GetById(long data)
        {
             HIS_TRACKING result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_TRACKING resultData = null;
                if (valid)
                {
                    resultData = new HisTrackingGet(param).GetById(data);
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

        
        public  HIS_TRACKING GetById(long data, HisTrackingFilterQuery filter)
        {
             HIS_TRACKING result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_TRACKING resultData = null;
                if (valid)
                {
                    resultData = new HisTrackingGet(param).GetById(data, filter);
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

        
        public  List<HIS_TRACKING> GetByIds(List<long> ids)
        {
             List<HIS_TRACKING> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(ids);
                List<HIS_TRACKING> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_TRACKING>();
                    var skip = 0;
                    while (ids.Count - skip > 0)
                    {
                        var Ids = ids.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisTrackingGet(param).GetByIds(Ids));
                    }
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

        
        public  List<HIS_TRACKING> GetByTreatmentId(long id)
        {
             List<HIS_TRACKING> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(id);
                List<HIS_TRACKING> resultData = null;
                if (valid)
                {
                    resultData = new HisTrackingGet(param).GetByTreatmentId(id);
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
