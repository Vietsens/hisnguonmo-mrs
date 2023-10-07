using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTestIndexUnit
{
    public partial class HisTestIndexUnitManager : BusinessBase
    {
        public HisTestIndexUnitManager()
            : base()
        {

        }

        public HisTestIndexUnitManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_TEST_INDEX_UNIT> Get(HisTestIndexUnitFilterQuery filter)
        {
             List<HIS_TEST_INDEX_UNIT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_TEST_INDEX_UNIT> resultData = null;
                if (valid)
                {
                    resultData = new HisTestIndexUnitGet(param).Get(filter);
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

        
        public  HIS_TEST_INDEX_UNIT GetById(long data)
        {
             HIS_TEST_INDEX_UNIT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_TEST_INDEX_UNIT resultData = null;
                if (valid)
                {
                    resultData = new HisTestIndexUnitGet(param).GetById(data);
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

        
        public  HIS_TEST_INDEX_UNIT GetById(long data, HisTestIndexUnitFilterQuery filter)
        {
             HIS_TEST_INDEX_UNIT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_TEST_INDEX_UNIT resultData = null;
                if (valid)
                {
                    resultData = new HisTestIndexUnitGet(param).GetById(data, filter);
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

        
        public  HIS_TEST_INDEX_UNIT GetByCode(string data)
        {
             HIS_TEST_INDEX_UNIT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);

                HIS_TEST_INDEX_UNIT resultData = null;
                if (valid)
                {
                    resultData = new HisTestIndexUnitGet(param).GetByCode(data);
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

        
        public  HIS_TEST_INDEX_UNIT GetByCode(string data, HisTestIndexUnitFilterQuery filter)
        {
             HIS_TEST_INDEX_UNIT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_TEST_INDEX_UNIT resultData = null;
                if (valid)
                {
                    resultData = new HisTestIndexUnitGet(param).GetByCode(data, filter);
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
