using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServ
{
	partial class HisSereServGet : GetBase
	{
		internal List<V_HIS_SERE_SERV_4> GetView4(HisSereServView4FilterQuery filter)
		{
			try
			{
				return DAOWorker.HisSereServDAO.GetView4(filter.Query(), param);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				param.HasException = true;
				return null;
			}
		}

		internal List<V_HIS_SERE_SERV_4> GetView4ByIds(List<long> ids)
		{
			try
			{
				if (ids != null)
				{
					HisSereServView4FilterQuery filter = new HisSereServView4FilterQuery();
					filter.IDs = ids;
					return this.GetView4(filter);
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

		internal List<V_HIS_SERE_SERV_4> GetView4ByServiceReqId(long serviceReqId)
		{
			HisSereServView4FilterQuery filter = new HisSereServView4FilterQuery();
			filter.SERVICE_REQ_ID = serviceReqId;
			return new HisSereServGet().GetView4(filter);
		}

		internal List<V_HIS_SERE_SERV_4> GetView4ByServiceReqIdAndIsSpecimen(long serviceReqId, bool isSpecimen)
		{
			HisSereServView4FilterQuery filter = new HisSereServView4FilterQuery();
			filter.SERVICE_REQ_ID = serviceReqId;
			filter.IS_SPECIMEN = isSpecimen;
			return new HisSereServGet().GetView4(filter);
		}

		internal List<V_HIS_SERE_SERV_4> GetView4ByServiceReqIdsAndIsSpecimen(List<long> serviceReqIds, bool isSpecimen)
		{
			if (serviceReqIds != null)
			{
				HisSereServView4FilterQuery filter = new HisSereServView4FilterQuery();
				filter.SERVICE_REQ_IDs = serviceReqIds;
				filter.IS_SPECIMEN = isSpecimen;
				return new HisSereServGet().GetView4(filter);
			}
			return null;
		}

		internal List<V_HIS_SERE_SERV_4> GetView4ByTreatmentId(long treatmentId)
		{
			HisSereServView4FilterQuery filter = new HisSereServView4FilterQuery();
			filter.TREATMENT_ID = treatmentId;
			return new HisSereServGet().GetView4(filter);
		}
		
		internal V_HIS_SERE_SERV_4 GetView4ById(long id)
		{
			try
			{
				return GetView4ById(id, new HisSereServView4FilterQuery());
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				param.HasException = true;
				return null;
			}
		}

		internal V_HIS_SERE_SERV_4 GetView4ById(long id, HisSereServView4FilterQuery filter)
		{
			try
			{
				return DAOWorker.HisSereServDAO.GetView4ById(id, filter.Query());
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
