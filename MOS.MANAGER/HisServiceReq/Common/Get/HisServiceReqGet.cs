using AutoMapper;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.MANAGER.HisServiceReq
{
    partial class HisServiceReqGet : GetBase
    {
        internal HisServiceReqGet()
            : base()
        {

        }

        internal HisServiceReqGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERVICE_REQ> Get(HisServiceReqFilterQuery filter)
        {
            try
            {
                List<HIS_SERVICE_REQ> result = DAOWorker.HisServiceReqDAO.Get(filter.Query(), param);
                if (IsNotNullOrEmpty(result))
                {
                    result.ForEach(o =>
                    {
                        o.HIS_SERVICE_REQ1 = null;
                        o.HIS_SERVICE_REQ2 = null;
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_REQ GetById(long id)
        {
            try
            {
                return GetById(id, new HisServiceReqFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_REQ GetById(long id, HisServiceReqFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceReqDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_REQ GetByCode(string code)
        {
            try
            {
                return DAOWorker.HisServiceReqDAO.GetByCode(code, new HisServiceReqFilterQuery().Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_REQ GetLastByCodeEndWith(string code)
        {
            try
            {
                HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
                filter.SERVICE_REQ_CODE__ENDS_WITH = code;
                filter.ORDER_FIELD = "ID";
                filter.ORDER_DIRECTION = "DESC";
                List<HIS_SERVICE_REQ> data = this.Get(filter);
                return IsNotNullOrEmpty(data) ? data[0] : null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_REQ GetByCode(string code, HisServiceReqFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceReqDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE_REQ> GetByIds(List<long> ids)
        {
            try
            {
                if (IsNotNullOrEmpty(ids))
                {
                    HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
                    filter.IDs = ids;
                    return this.Get(filter);
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

        internal List<HIS_SERVICE_REQ> GetByDepartmentId(long departmentId)
        {
            try
            {
                HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
                filter.REQUEST_DEPARTMENT_ID__OR__EXECUTE_DEPARTMENT_ID = departmentId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE_REQ> GetByRoomId(long roomId)
        {
            try
            {
                HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
                filter.REQUEST_ROOM_ID__OR__EXECUTE_ROOM_ID = roomId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE_REQ> GetByPatientId(long id)
        {
            try
            {
                HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
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

        internal List<HIS_SERVICE_REQ> GetByServiceReqSttId(long id)
        {
            try
            {
                HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
                filter.SERVICE_REQ_STT_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE_REQ> GetByServiceReqTypeId(long id)
        {
            try
            {
                HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
                filter.SERVICE_REQ_TYPE_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE_REQ> GetByTreatmentId(long id)
        {
            try
            {
                HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
                filter.TREATMENT_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERVICE_REQ GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisServiceReqViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERVICE_REQ GetViewById(long id, HisServiceReqViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceReqDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERVICE_REQ> GetView(HisServiceReqViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceReqDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERVICE_REQ> GetViewByIds(List<long> ids)
        {
            try
            {
                if (IsNotNullOrEmpty(ids))
                {
                    HisServiceReqViewFilterQuery filter = new HisServiceReqViewFilterQuery();
                    filter.IDs = ids;
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

        internal List<V_HIS_SERVICE_REQ> GetViewByTreatmentId(long id)
        {
            try
            {
                HisServiceReqViewFilterQuery filter = new HisServiceReqViewFilterQuery();
                filter.TREATMENT_ID = id;
                return this.GetView(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE_REQ> GetByExecuteGroupId(long id)
        {
            try
            {
                HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
                filter.EXECUTE_GROUP_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE_REQ> GetByInstructionTime(long instructionTimeFrom, long instructionTimeTo, List<long> serviceReqTypeIds, List<long> treatmentIds)
        {
            try
            {
                HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
                filter.SERVICE_REQ_TYPE_IDs = serviceReqTypeIds;
                filter.INTRUCTION_TIME_FROM = instructionTimeFrom;
                filter.INTRUCTION_TIME_TO = instructionTimeTo;
                filter.TREATMENT_IDs = treatmentIds;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE_REQ> GetByTrackingId(long id)
        {
            HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
            filter.TRACKING_ID = id;
            return this.Get(filter);
        }

        internal List<HIS_SERVICE_REQ> GetByParentId(long parentId)
        {
            HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
            filter.PARENT_ID = parentId;
            return this.Get(filter);
        }

        internal HIS_SERVICE_REQ GetByBarcode(string barcode)
        {
            if (!string.IsNullOrWhiteSpace(barcode))
            {
                HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
                filter.BARCODE__EXACT = barcode;
                List<HIS_SERVICE_REQ> list = this.Get(filter);
                return IsNotNullOrEmpty(list) ? list[0] : null;
            }
            return null;
        }

        internal HIS_SERVICE_REQ GetByServiceReqCode(string serviceReqCode)
        {
            if (!string.IsNullOrWhiteSpace(serviceReqCode))
            {
                HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
                filter.SERVICE_REQ_CODE__EXACT = serviceReqCode;
                List<HIS_SERVICE_REQ> list = this.Get(filter);
                return IsNotNullOrEmpty(list) ? list[0] : null;
            }
            return null;
        }

        internal List<HIS_SERVICE_REQ> GetByPaanPositionId(long paanPositionId)
        {
            HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
            filter.PAAN_POSITION_ID = paanPositionId;
            return this.Get(filter);
        }

        internal List<HIS_SERVICE_REQ> GetByPaanLiquidId(long paanLiquidId)
        {
            HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
            filter.PAAN_LIQUID_ID = paanLiquidId;
            return this.Get(filter);
        }

        internal List<HIS_SERVICE_REQ> GetByRehaSumId(long rehaSumId)
        {
            HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
            filter.REHA_SUM_ID = rehaSumId;
            return this.Get(filter);
        }

        internal List<HIS_SERVICE_REQ> GetByDhstId(long id)
        {
            HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
            filter.DHST_ID = id;
            return this.Get(filter);
        }

        internal List<HIS_SERVICE_REQ> GetByTreatmentIds(List<long> ids)
        {
            HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
            filter.TREATMENT_IDs = ids;
            return this.Get(filter);
        }
    }
}
