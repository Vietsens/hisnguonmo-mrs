using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisReportTypeCat
{
    public partial class HisReportTypeCatManager : BusinessBase
    {
        public HisReportTypeCatManager()
            : base()
        {

        }

        public HisReportTypeCatManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_REPORT_TYPE_CAT> Get(HisReportTypeCatFilterQuery filter)
        {
             List<HIS_REPORT_TYPE_CAT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_REPORT_TYPE_CAT> resultData = null;
                if (valid)
                {
                    resultData = new HisReportTypeCatGet(param).Get(filter);
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

        
        public  HIS_REPORT_TYPE_CAT GetById(long data)
        {
             HIS_REPORT_TYPE_CAT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_REPORT_TYPE_CAT resultData = null;
                if (valid)
                {
                    resultData = new HisReportTypeCatGet(param).GetById(data);
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

        
        public  HIS_REPORT_TYPE_CAT GetById(long data, HisReportTypeCatFilterQuery filter)
        {
             HIS_REPORT_TYPE_CAT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_REPORT_TYPE_CAT resultData = null;
                if (valid)
                {
                    resultData = new HisReportTypeCatGet(param).GetById(data, filter);
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
