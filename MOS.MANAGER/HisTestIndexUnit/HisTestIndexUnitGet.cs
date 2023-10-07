using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTestIndexUnit
{
    class HisTestIndexUnitGet : GetBase
    {
        internal HisTestIndexUnitGet()
            : base()
        {

        }

        internal HisTestIndexUnitGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_TEST_INDEX_UNIT> Get(HisTestIndexUnitFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTestIndexUnitDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TEST_INDEX_UNIT GetById(long id)
        {
            try
            {
                return GetById(id, new HisTestIndexUnitFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TEST_INDEX_UNIT GetById(long id, HisTestIndexUnitFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTestIndexUnitDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TEST_INDEX_UNIT GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisTestIndexUnitFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TEST_INDEX_UNIT GetByCode(string code, HisTestIndexUnitFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTestIndexUnitDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
