using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServ
{
	partial class HisSereServGet : GetBase
	{
		internal List<V_HIS_SERE_SERV_2> GetView2(HisSereServView2FilterQuery filter)
		{
			try
			{
				return DAOWorker.HisSereServDAO.GetView2(filter.Query(), param);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				param.HasException = true;
				return null;
			}
		}

        internal List<V_HIS_SERE_SERV_2> GetView2ByIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisSereServView2FilterQuery filter = new HisSereServView2FilterQuery();
                    filter.IDs = ids;
                    return this.GetView2(filter);
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

		internal List<V_HIS_SERE_SERV_2> GetView2ByServiceReqId(long serviceReqId)
		{
			HisSereServView2FilterQuery filter = new HisSereServView2FilterQuery();
			filter.SERVICE_REQ_ID = serviceReqId;
			return new HisSereServGet().GetView2(filter);
		}

        internal List<V_HIS_SERE_SERV_2> GetView2ByServiceReqIdAndIsSpecimen(long serviceReqId, bool isSpecimen)
        {
            HisSereServView2FilterQuery filter = new HisSereServView2FilterQuery();
            filter.SERVICE_REQ_ID = serviceReqId;
            filter.IS_SPECIMEN = isSpecimen;
            return new HisSereServGet().GetView2(filter);
        }

        internal List<V_HIS_SERE_SERV_2> GetView2ByServiceReqIdsAndIsSpecimen(List<long> serviceReqIds, bool isSpecimen)
        {
            if (serviceReqIds != null)
            {
                HisSereServView2FilterQuery filter = new HisSereServView2FilterQuery();
                filter.SERVICE_REQ_IDs = serviceReqIds;
                filter.IS_SPECIMEN = isSpecimen;
                return new HisSereServGet().GetView2(filter);
            }
            return null;
        }

        internal List<V_HIS_SERE_SERV_2> GetView2ByTreatmentId(long treatmentId)
        {
            HisSereServView2FilterQuery filter = new HisSereServView2FilterQuery();
            filter.TREATMENT_ID = treatmentId;
            return new HisSereServGet().GetView2(filter);
        }

        internal List<V_HIS_SERE_SERV_2> GetView2ByHeinApprovalId(long heinApprovalId)
        {
            HisSereServView2FilterQuery filter = new HisSereServView2FilterQuery();
            filter.HEIN_APPROVAL_ID = heinApprovalId;
            return new HisSereServGet().GetView2(filter);
        }
		
        internal V_HIS_SERE_SERV_2 GetView2ById(long id)
        {
            try
            {
                return GetView2ById(id, new HisSereServView2FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERE_SERV_2 GetView2ById(long id, HisSereServView2FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServDAO.GetView2ById(id, filter.Query());
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
