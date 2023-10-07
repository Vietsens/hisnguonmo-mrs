using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTestIndexRange
{
    public class HisTestIndexRangeManager : BusinessBase
    {
        public HisTestIndexRangeManager()
            : base()
        {

        }

        public HisTestIndexRangeManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_TEST_INDEX_RANGE> Get(HisTestIndexRangeFilterQuery filter)
        {
            List<HIS_TEST_INDEX_RANGE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_TEST_INDEX_RANGE> resultData = null;
                if (valid)
                {
                    resultData = new HisTestIndexRangeGet(param).Get(filter);
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

        
        public List<V_HIS_TEST_INDEX_RANGE> GetView(HisTestIndexRangeViewFilterQuery filter)
        {
            List<V_HIS_TEST_INDEX_RANGE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_TEST_INDEX_RANGE> resultData = null;
                if (valid)
                {
                    resultData = new HisTestIndexRangeGet(param).GetView(filter);
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
