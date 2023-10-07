using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServ
{
	partial class HisSereServGet : GetBase
	{
		internal List<V_HIS_SERE_SERV_3> GetView3(HisSereServView3FilterQuery filter)
		{
			try
			{
				return DAOWorker.HisSereServDAO.GetView3(filter.Query(), param);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				param.HasException = true;
				return null;
			}
		}

		internal List<V_HIS_SERE_SERV_3> GetView3ByIds(List<long> ids)
		{
			try
			{
				if (ids != null)
				{
					HisSereServView3FilterQuery filter = new HisSereServView3FilterQuery();
					filter.IDs = ids;
					return this.GetView3(filter);
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

		internal List<V_HIS_SERE_SERV_3> GetView3ByServiceReqId(long serviceReqId)
		{
			HisSereServView3FilterQuery filter = new HisSereServView3FilterQuery();
			filter.SERVICE_REQ_ID = serviceReqId;
			return new HisSereServGet().GetView3(filter);
		}

		internal List<V_HIS_SERE_SERV_3> GetView3ByServiceReqIdAndIsSpecimen(long serviceReqId, bool isSpecimen)
		{
			HisSereServView3FilterQuery filter = new HisSereServView3FilterQuery();
			filter.SERVICE_REQ_ID = serviceReqId;
			filter.IS_SPECIMEN = isSpecimen;
			return new HisSereServGet().GetView3(filter);
		}

		internal List<V_HIS_SERE_SERV_3> GetView3ByServiceReqIdsAndIsSpecimen(List<long> serviceReqIds, bool isSpecimen)
		{
			if (serviceReqIds != null)
			{
				HisSereServView3FilterQuery filter = new HisSereServView3FilterQuery();
				filter.SERVICE_REQ_IDs = serviceReqIds;
				filter.IS_SPECIMEN = isSpecimen;
				return new HisSereServGet().GetView3(filter);
			}
			return null;
		}

		internal List<V_HIS_SERE_SERV_3> GetView3ByTreatmentId(long treatmentId)
		{
			HisSereServView3FilterQuery filter = new HisSereServView3FilterQuery();
			filter.TREATMENT_ID = treatmentId;
			return new HisSereServGet().GetView3(filter);
		}
		
		internal V_HIS_SERE_SERV_3 GetView3ById(long id)
		{
			try
			{
				return GetView3ById(id, new HisSereServView3FilterQuery());
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				param.HasException = true;
				return null;
			}
		}

		internal V_HIS_SERE_SERV_3 GetView3ById(long id, HisSereServView3FilterQuery filter)
		{
			try
			{
				return DAOWorker.HisSereServDAO.GetView3ById(id, filter.Query());
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
