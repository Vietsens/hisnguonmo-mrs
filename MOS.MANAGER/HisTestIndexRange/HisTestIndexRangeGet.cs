using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTestIndexRange
{
    class HisTestIndexRangeGet : GetBase
    {
        internal HisTestIndexRangeGet()
            : base()
        {

        }

        internal HisTestIndexRangeGet(Inventec.Core.CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_TEST_INDEX_RANGE> Get(HisTestIndexRangeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTestIndexRangeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_TEST_INDEX_RANGE> GetView(HisTestIndexRangeViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTestIndexRangeDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TEST_INDEX_RANGE GetById(long id)
        {
            try
            {
                return GetById(id, new HisTestIndexRangeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TEST_INDEX_RANGE GetById(long id, HisTestIndexRangeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTestIndexRangeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TEST_INDEX_RANGE GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisTestIndexRangeViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TEST_INDEX_RANGE GetViewById(long id, HisTestIndexRangeViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTestIndexRangeDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_TEST_INDEX_RANGE> GetByTestIndexId(long testIndexId)
        {
            try
            {
                HisTestIndexRangeFilterQuery filter = new HisTestIndexRangeFilterQuery();
                filter.TEST_INDEX_ID = testIndexId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_TEST_INDEX_RANGE> GetActive()
        {
            try
            {
                HisTestIndexRangeFilterQuery filter = new HisTestIndexRangeFilterQuery();
                filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                return this.Get(filter);
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
