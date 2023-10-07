using AutoMapper;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMest
{
    partial class HisImpMestGet : GetBase
    {
        internal HisImpMestGet()
            : base()
        {

        }

        internal HisImpMestGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_IMP_MEST> Get(HisImpMestFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_IMP_MEST> GetView(HisImpMestViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_MEST GetById(long id)
        {
            try
            {
                return GetById(id, new HisImpMestFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_IMP_MEST> GetByAggrImpMestId(long aggrImpMestId)
        {
            try
            {
                HisImpMestFilterQuery filter = new HisImpMestFilterQuery();
                filter.AGGR_IMP_MEST_ID = aggrImpMestId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_MEST GetById(long id, HisImpMestFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_IMP_MEST GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisImpMestViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_IMP_MEST GetViewById(long id, HisImpMestViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_MEST GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisImpMestFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_MEST GetByCode(string code, HisImpMestFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_IMP_MEST GetViewByCode(string code)
        {
            try
            {
                return GetViewByCode(code, new HisImpMestViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_IMP_MEST GetViewByCode(string code, HisImpMestViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestDAO.GetViewByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_IMP_MEST> GetByReqDepartmentId(long departmentId)
        {
            try
            {
                HisImpMestFilterQuery filter = new HisImpMestFilterQuery();
                filter.REQ_DEPARTMENT_ID = departmentId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_IMP_MEST> GetByRoomId(long roomId)
        {
            try
            {
                HisImpMestFilterQuery filter = new HisImpMestFilterQuery();
                filter.REQ_ROOM_ID = roomId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_IMP_MEST> GetByMediStockId(long id)
        {
            try
            {
                HisImpMestFilterQuery filter = new HisImpMestFilterQuery();
                filter.MEDI_STOCK_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_IMP_MEST> GetByMediStockPeriodId(long id)
        {
            HisImpMestFilterQuery filter = new HisImpMestFilterQuery();
            filter.MEDI_STOCK_PERIOD_ID = id;
            return this.Get(filter);
        }

        internal List<HIS_IMP_MEST> GetByIds(List<long> ids)
        {
            if (IsNotNullOrEmpty(ids))
            {
                HisImpMestFilterQuery filter = new HisImpMestFilterQuery();
                filter.IDs = ids;
                return this.Get(filter);
            }
            return null;
        }

        internal List<HIS_IMP_MEST> GetByMobaExpMestId(long expMestId)
        {
            HisImpMestFilterQuery filter = new HisImpMestFilterQuery();
            filter.MOBA_EXP_MEST_ID = expMestId;
            return this.Get(filter);
        }

        internal List<HIS_IMP_MEST> GetByMobaExpMestIds(List<long> expMestIds)
        {
            if (IsNotNullOrEmpty(expMestIds))
            {
                HisImpMestFilterQuery filter = new HisImpMestFilterQuery();
                filter.MOBA_EXP_MEST_IDs = expMestIds;
                return this.Get(filter);
            }
            return null;
        }

        internal List<HIS_IMP_MEST> GetBySupplierId(long supplierId)
        {
            HisImpMestFilterQuery filter = new HisImpMestFilterQuery();
            filter.SUPPLIER_ID = supplierId;
            return this.Get(filter);
        }

        internal List<HIS_IMP_MEST> GetByChmsExpMestId(long expMestId)
        {
            try
            {
                HisImpMestFilterQuery filter = new HisImpMestFilterQuery();
                filter.CHMS_EXP_MEST_ID = expMestId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_IMP_MEST> GetViewByIds(List<long> ids)
        {
            if (IsNotNullOrEmpty(ids))
            {
                HisImpMestViewFilterQuery filter = new HisImpMestViewFilterQuery();
                filter.IDs = ids;
                return this.GetView(filter);
            }
            return null;
        }
    }
}
