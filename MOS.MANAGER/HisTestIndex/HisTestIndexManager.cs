using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTestIndex
{
    partial class HisTestIndexManager : BusinessBase
    {
        public HisTestIndexManager()
            : base()
        {

        }

        public HisTestIndexManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_TEST_INDEX> Get(HisTestIndexFilterQuery filter)
        {
             List<HIS_TEST_INDEX> result = null;

            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_TEST_INDEX> resultData = null;
                if (valid)
                {
                    resultData = new HisTestIndexGet(param).Get(filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }

            return result;
        }

        
        public  List<V_HIS_TEST_INDEX> GetView(HisTestIndexViewFilterQuery filter)
        {
             List<V_HIS_TEST_INDEX> result = null;

            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_TEST_INDEX> resultData = null;
                if (valid)
                {
                    resultData = new HisTestIndexGet(param).GetView(filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }

            return result;
        }
    }
}
