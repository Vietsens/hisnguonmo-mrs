using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServ
{
	partial class HisSereServGet : GetBase
	{
		internal List<V_HIS_SERE_SERV_6> GetView6(HisSereServView6FilterQuery filter)
		{
			try
			{
				return DAOWorker.HisSereServDAO.GetView6(filter.Query(), param);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				param.HasException = true;
				return null;
			}
		}

		internal List<V_HIS_SERE_SERV_6> GetView6ByIds(List<long> ids)
		{
			try
			{
				if (ids != null)
				{
					HisSereServView6FilterQuery filter = new HisSereServView6FilterQuery();
					filter.IDs = ids;
					return this.GetView6(filter);
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

		internal List<V_HIS_SERE_SERV_6> GetView6ByServiceReqId(long serviceReqId)
		{
			HisSereServView6FilterQuery filter = new HisSereServView6FilterQuery();
			filter.SERVICE_REQ_ID = serviceReqId;
			return new HisSereServGet().GetView6(filter);
		}

		internal List<V_HIS_SERE_SERV_6> GetView6ByServiceReqIdAndIsSpecimen(long serviceReqId, bool isSpecimen)
		{
			HisSereServView6FilterQuery filter = new HisSereServView6FilterQuery();
			filter.SERVICE_REQ_ID = serviceReqId;
			filter.IS_SPECIMEN = isSpecimen;
			return new HisSereServGet().GetView6(filter);
		}

		internal List<V_HIS_SERE_SERV_6> GetView6ByServiceReqIdsAndIsSpecimen(List<long> serviceReqIds, bool isSpecimen)
		{
			if (serviceReqIds != null)
			{
				HisSereServView6FilterQuery filter = new HisSereServView6FilterQuery();
				filter.SERVICE_REQ_IDs = serviceReqIds;
				filter.IS_SPECIMEN = isSpecimen;
				return new HisSereServGet().GetView6(filter);
			}
			return null;
		}
		
		internal V_HIS_SERE_SERV_6 GetView6ById(long id)
		{
			try
			{
				return GetView6ById(id, new HisSereServView6FilterQuery());
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				param.HasException = true;
				return null;
			}
		}

		internal V_HIS_SERE_SERV_6 GetView6ById(long id, HisSereServView6FilterQuery filter)
		{
			try
			{
				return DAOWorker.HisSereServDAO.GetView6ById(id, filter.Query());
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
