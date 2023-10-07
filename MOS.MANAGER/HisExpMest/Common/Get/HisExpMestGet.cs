using AutoMapper;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMest.Common.Get
{
    partial class HisExpMestGet : GetBase
    {
        internal HisExpMestGet()
            : base()
        {

        }

        internal HisExpMestGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EXP_MEST> Get(HisExpMestFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EXP_MEST> GetView(HisExpMestViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST GetById(long id)
        {
            try
            {
                return GetById(id, new HisExpMestFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST GetByServiceReqId(long serviceReqId)
        {
            try
            {
                HisExpMestFilterQuery filter = new HisExpMestFilterQuery();
                filter.SERVICE_REQ_ID = serviceReqId;
                List<HIS_EXP_MEST> data = this.Get(filter);
                return IsNotNull(data) ? data[0] : null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EXP_MEST> GetByAggrExpMestId(long aggrExpMestId)
        {
            try
            {
                HisExpMestFilterQuery filter = new HisExpMestFilterQuery();
                filter.AGGR_EXP_MEST_ID = aggrExpMestId;
                return Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EXP_MEST> GetByIds(List<long> ids)
        {
            if (ids != null)
            {
                HisExpMestFilterQuery filter = new HisExpMestFilterQuery();
                filter.IDs = ids;
                return Get(filter);
            }
            return null;
        }

        internal HIS_EXP_MEST GetById(long id, HisExpMestFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisExpMestViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST GetViewById(long id, HisExpMestViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisExpMestFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST GetByCode(string code, HisExpMestFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST GetViewByCode(string code)
        {
            try
            {
                return GetViewByCode(code, new HisExpMestViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST GetViewByCode(string code, HisExpMestViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestDAO.GetViewByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EXP_MEST> GetByReqDepartmentId(long departmentId)
        {
            try
            {
                HisExpMestFilterQuery filter = new HisExpMestFilterQuery();
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

        internal List<HIS_EXP_MEST> GetByRoomId(long roomId)
        {
            try
            {
                HisExpMestFilterQuery filter = new HisExpMestFilterQuery();
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

        internal List<HIS_EXP_MEST> GetByMediStockId(long id)
        {
            try
            {
                HisExpMestFilterQuery filter = new HisExpMestFilterQuery();
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

        internal List<HIS_EXP_MEST> GetByMediStockIdOrImpMediStockId(long id)
        {
            try
            {
                HisExpMestFilterQuery filter = new HisExpMestFilterQuery();
                filter.MEDI_STOCK_ID__OR__IMP_MEDI_STOCK_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EXP_MEST> GetByPatientId(long id)
        {
            try
            {
                HisExpMestFilterQuery filter = new HisExpMestFilterQuery();
                filter.TDL_PATIENT_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EXP_MEST> GetByTreatmentId(long treatmentId)
        {
            HisExpMestFilterQuery filter = new HisExpMestFilterQuery();
            filter.TDL_TREATMENT_ID = treatmentId;
            return this.Get(filter);
        }

        internal List<HIS_EXP_MEST> GetByServiceReqIds(List<long> serviceReqIds)
        {
            if (IsNotNullOrEmpty(serviceReqIds))
            {
                HisExpMestFilterQuery filter = new HisExpMestFilterQuery();
                filter.SERVICE_REQ_IDs = serviceReqIds;
                return this.Get(filter);
            }
            return null;
        }

        internal List<HIS_EXP_MEST> GetBySupplierId(long supplierId)
        {
            HisExpMestFilterQuery filter = new HisExpMestFilterQuery();
            filter.SUPPLIER_ID = supplierId;
            return this.Get(filter);
        }

        internal List<HIS_EXP_MEST> GetByBillId(long billId)
        {
            try
            {
                HisExpMestFilterQuery filter = new HisExpMestFilterQuery();
                filter.BILL_ID = billId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EXP_MEST> GetByXbttExpMestId(long expMestId)
        {
            try
            {
                HisExpMestFilterQuery filter = new HisExpMestFilterQuery();
                filter.XBTT_EXP_MEST_ID = expMestId;
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
