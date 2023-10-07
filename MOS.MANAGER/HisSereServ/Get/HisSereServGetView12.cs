using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServ
{
	partial class HisSereServGet : GetBase
	{
		internal List<V_HIS_SERE_SERV_12> GetView12(HisSereServView12FilterQuery filter)
		{
			try
			{
				return DAOWorker.HisSereServDAO.GetView12(filter.Query(), param);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				param.HasException = true;
				return null;
			}
		}

		internal List<V_HIS_SERE_SERV_12> GetView12ByIds(List<long> ids)
		{
			try
			{
				if (ids != null)
				{
					HisSereServView12FilterQuery filter = new HisSereServView12FilterQuery();
					filter.IDs = ids;
					return this.GetView12(filter);
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

		internal List<V_HIS_SERE_SERV_12> GetView12ByServiceReqId(long serviceReqId)
		{
			HisSereServView12FilterQuery filter = new HisSereServView12FilterQuery();
			filter.SERVICE_REQ_ID = serviceReqId;
			return new HisSereServGet().GetView12(filter);
		}

		internal List<V_HIS_SERE_SERV_12> GetView12ByServiceReqIdAndIsSpecimen(long serviceReqId, bool isSpecimen)
		{
			HisSereServView12FilterQuery filter = new HisSereServView12FilterQuery();
			filter.SERVICE_REQ_ID = serviceReqId;
			filter.IS_SPECIMEN = isSpecimen;
			return new HisSereServGet().GetView12(filter);
		}

		internal List<V_HIS_SERE_SERV_12> GetView12ByServiceReqIdsAndIsSpecimen(List<long> serviceReqIds, bool isSpecimen)
		{
			if (serviceReqIds != null)
			{
				HisSereServView12FilterQuery filter = new HisSereServView12FilterQuery();
				filter.SERVICE_REQ_IDs = serviceReqIds;
				filter.IS_SPECIMEN = isSpecimen;
				return new HisSereServGet().GetView12(filter);
			}
			return null;
		}
		
		internal V_HIS_SERE_SERV_12 GetView12ById(long id)
		{
			try
			{
				return GetView12ById(id, new HisSereServView12FilterQuery());
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				param.HasException = true;
				return null;
			}
		}

		internal V_HIS_SERE_SERV_12 GetView12ById(long id, HisSereServView12FilterQuery filter)
		{
			try
			{
				return DAOWorker.HisSereServDAO.GetView12ById(id, filter.Query());
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
