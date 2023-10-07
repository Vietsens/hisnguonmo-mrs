using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.MANAGER.HisTestIndex
{
    partial class HisTestIndexGet : GetBase
    {
        internal HisTestIndexGet()
            : base()
        {

        }

        internal HisTestIndexGet(Inventec.Core.CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_TEST_INDEX> Get(HisTestIndexFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTestIndexDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_TEST_INDEX> GetView(HisTestIndexViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTestIndexDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_TEST_INDEX> GetViewByServiceIds(List<long> serviceIds)
        {
            try
            {
                if (serviceIds != null)
                {
                    HisTestIndexViewFilterQuery filter = new HisTestIndexViewFilterQuery();
                    filter.SERVICE_IDs = serviceIds;
                    return this.GetView(filter);
                }
                return null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_TEST_INDEX> GetActiveViewByServiceIds(List<long> serviceIds)
        {
            try
            {
                if (serviceIds != null)
                {
                    HisTestIndexViewFilterQuery filter = new HisTestIndexViewFilterQuery();
                    filter.SERVICE_IDs = serviceIds;
                    filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                    return this.GetView(filter);
                }
                return null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TEST_INDEX GetById(long id)
        {
            try
            {
                return GetById(id, new HisTestIndexFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TEST_INDEX GetById(long id, HisTestIndexFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTestIndexDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TEST_INDEX GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisTestIndexViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TEST_INDEX GetViewById(long id, HisTestIndexViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTestIndexDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TEST_INDEX GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisTestIndexFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TEST_INDEX GetByCode(string code, HisTestIndexFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTestIndexDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TEST_INDEX GetViewByCode(string code)
        {
            try
            {
                return GetViewByCode(code, new HisTestIndexViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TEST_INDEX GetViewByCode(string code, HisTestIndexViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTestIndexDAO.GetViewByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_TEST_INDEX> GetByTestIndexUnitId(long id)
        {
            try
            {
                HisTestIndexFilterQuery filter = new HisTestIndexFilterQuery();
                filter.TEST_INDEX_UNIT_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_TEST_INDEX> GetActiveView()
        {
            try
            {
                HisTestIndexViewFilterQuery filter = new HisTestIndexViewFilterQuery();
                filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                return this.GetView(filter);
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
