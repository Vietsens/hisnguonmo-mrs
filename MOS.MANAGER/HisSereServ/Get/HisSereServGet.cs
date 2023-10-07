using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.LibraryHein.Bhyt;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServ
{
    partial class HisSereServGet : GetBase
    {
        internal HisSereServGet()
            : base()
        {

        }

        internal HisSereServGet(Inventec.Core.CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERE_SERV> Get(HisSereServFilterQuery filter)
        {
            try
            {
                List<HIS_SERE_SERV> result = DAOWorker.HisSereServDAO.Get(filter.Query(), param);
                if (IsNotNullOrEmpty(result))
                {
                    result.ForEach(o =>
                    {
                        o.HIS_SERE_SERV1 = null;
                        o.HIS_SERE_SERV2 = null;
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

        internal List<V_HIS_SERE_SERV> GetView(HisSereServViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERE_SERV GetById(long id)
        {
            try
            {
                return GetById(id, new HisSereServFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERE_SERV> GetViewByIds(List<long> ids)
        {
            if (ids != null)
            {
                HisSereServViewFilterQuery filter = new HisSereServViewFilterQuery();
                filter.IDs = ids;
                return this.GetView(filter);
            }
            return null;
        }

        internal List<V_HIS_SERE_SERV> GetViewByServiceReqIds(List<long> serviceReqIds)
        {
            if (IsNotNullOrEmpty(serviceReqIds))
            {
                HisSereServViewFilterQuery filter = new HisSereServViewFilterQuery();
                filter.SERVICE_REQ_IDs = serviceReqIds;
                return this.GetView(filter);
            }
            return null;
        }

        internal List<HIS_SERE_SERV> GetByIds(List<long> ids)
        {
            if (ids != null)
            {
                HisSereServFilterQuery filter = new HisSereServFilterQuery();
                filter.IDs = ids;
                return this.Get(filter);
            }
            return null;
        }

        internal List<HIS_SERE_SERV> GetByInvoiceId(long invoiceId)
        {
            HisSereServFilterQuery filter = new HisSereServFilterQuery();
            filter.INVOICE_ID = invoiceId;
            return this.Get(filter);
        }

        internal List<HIS_SERE_SERV> GetByServiceReqId(long serviceReqId)
        {
            HisSereServFilterQuery filter = new HisSereServFilterQuery();
            filter.SERVICE_REQ_ID = serviceReqId;
            return new HisSereServGet().Get(filter);
        }

        internal List<HIS_SERE_SERV> GetByServiceReqIds(List<long> serviceReqIds)
        {
            if (IsNotNullOrEmpty(serviceReqIds))
            {
                HisSereServFilterQuery filter = new HisSereServFilterQuery();
                filter.SERVICE_REQ_IDs = serviceReqIds;
                return new HisSereServGet().Get(filter);
            }
            return null;
        }

        internal List<HIS_SERE_SERV> GetByServiceReqId(List<long> serviceReqIds)
        {
            HisSereServFilterQuery filter = new HisSereServFilterQuery();
            filter.SERVICE_REQ_IDs = serviceReqIds;
            return new HisSereServGet().Get(filter);
        }

        internal List<V_HIS_SERE_SERV> GetViewByServiceReqId(long serviceReqId)
        {
            HisSereServViewFilterQuery filter = new HisSereServViewFilterQuery();
            filter.SERVICE_REQ_ID = serviceReqId;
            return new HisSereServGet().GetView(filter);
        }

        internal List<V_HIS_SERE_SERV> GetViewByServiceReqIdAndIsSpecimen(long serviceReqId, bool? isSpecimen)
        {
            HisSereServViewFilterQuery filter = new HisSereServViewFilterQuery();
            filter.SERVICE_REQ_ID = serviceReqId;
            filter.IS_SPECIMEN = isSpecimen;
            return new HisSereServGet().GetView(filter);
        }

        internal List<V_HIS_SERE_SERV> GetViewByServiceReqIdsAndIsSpecimen(List<long> serviceReqIds, bool? isSpecimen)
        {
            if (serviceReqIds != null)
            {
                HisSereServViewFilterQuery filter = new HisSereServViewFilterQuery();
                filter.SERVICE_REQ_IDs = serviceReqIds;
                filter.IS_SPECIMEN = isSpecimen;
                return new HisSereServGet().GetView(filter);
            }
            return null;
        }

        internal List<HIS_SERE_SERV> GetByServiceReqIdsAndIsSpecimen(List<long> serviceReqIds, bool? isSpecimen)
        {
            if (serviceReqIds != null)
            {
                HisSereServFilterQuery filter = new HisSereServFilterQuery();
                filter.SERVICE_REQ_IDs = serviceReqIds;
                filter.IS_SPECIMEN = isSpecimen;
                return new HisSereServGet().Get(filter);
            }
            return null;
        }

        internal List<V_HIS_SERE_SERV> GetViewByTreatmentId(long treatmentId)
        {
            HisSereServViewFilterQuery filter = new HisSereServViewFilterQuery();
            filter.TREATMENT_ID = treatmentId;
            return new HisSereServGet().GetView(filter);
        }

        internal HIS_SERE_SERV GetById(long id, HisSereServFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERE_SERV> GetByPatientTypeId(long patientTypeId)
        {
            HisSereServFilterQuery filter = new HisSereServFilterQuery();
            filter.PATIENT_TYPE_ID = patientTypeId;
            return this.Get(filter);
        }

        internal List<HIS_SERE_SERV> GetByTreatmentIdAndPatientTypeId(long treatmentId, long patientTypeId)
        {
            HisSereServFilterQuery filter = new HisSereServFilterQuery();
            filter.PATIENT_TYPE_ID = patientTypeId;
            filter.TREATMENT_ID = treatmentId;
            return this.Get(filter);
        }

        internal List<HIS_SERE_SERV> GetByServiceId(long serviceId)
        {
            HisSereServFilterQuery filter = new HisSereServFilterQuery();
            filter.SERVICE_ID = serviceId;
            return this.Get(filter);
        }

        internal List<HIS_SERE_SERV> GetByTreatmentId(long id)
        {
            HisSereServFilterQuery filter = new HisSereServFilterQuery();
            filter.TREATMENT_ID = id;
            return this.Get(filter);
        }

        internal List<HIS_SERE_SERV> GetByParentId(long parentId)
        {
            HisSereServFilterQuery filter = new HisSereServFilterQuery();
            filter.PARENT_ID = parentId;
            return this.Get(filter);
        }

        internal List<HIS_SERE_SERV> GetByParentIds(List<long> parentIds)
        {
            if (IsNotNullOrEmpty(parentIds))
            {
                HisSereServFilterQuery filter = new HisSereServFilterQuery();
                filter.PARENT_IDs = parentIds;
                return this.Get(filter);
            }
            return null;
        }

        internal V_HIS_SERE_SERV GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisSereServViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERE_SERV GetViewById(long id, HisSereServViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERE_SERV> GetByPackageId(long id)
        {
            HisSereServFilterQuery filter = new HisSereServFilterQuery();
            filter.PACKAGE_ID = id;
            return this.Get(filter);
        }

        internal List<HIS_SERE_SERV> GetByHeinApprovalId(long heinApprovalId)
        {
            HisSereServFilterQuery filter = new HisSereServFilterQuery();
            filter.HEIN_APPROVAL_ID = heinApprovalId;
            return this.Get(filter);
        }
    }
}
