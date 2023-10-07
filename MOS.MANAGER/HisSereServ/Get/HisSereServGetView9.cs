using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServ
{
	partial class HisSereServGet : GetBase
	{
		internal List<V_HIS_SERE_SERV_9> GetView9(HisSereServView9FilterQuery filter)
		{
			try
			{
				return DAOWorker.HisSereServDAO.GetView9(filter.Query(), param);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				param.HasException = true;
				return null;
			}
		}

		internal List<V_HIS_SERE_SERV_9> GetView9ByIds(List<long> ids)
		{
			try
			{
				if (ids != null)
				{
					HisSereServView9FilterQuery filter = new HisSereServView9FilterQuery();
					filter.IDs = ids;
					return this.GetView9(filter);
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

		internal List<V_HIS_SERE_SERV_9> GetView9ByServiceReqId(long serviceReqId)
		{
			HisSereServView9FilterQuery filter = new HisSereServView9FilterQuery();
			filter.SERVICE_REQ_ID = serviceReqId;
			return new HisSereServGet().GetView9(filter);
		}

		internal List<V_HIS_SERE_SERV_9> GetView9ByServiceReqIdAndIsSpecimen(long serviceReqId, bool isSpecimen)
		{
			HisSereServView9FilterQuery filter = new HisSereServView9FilterQuery();
			filter.SERVICE_REQ_ID = serviceReqId;
			filter.IS_SPECIMEN = isSpecimen;
			return new HisSereServGet().GetView9(filter);
		}

		internal List<V_HIS_SERE_SERV_9> GetView9ByServiceReqIdsAndIsSpecimen(List<long> serviceReqIds, bool isSpecimen)
		{
			if (serviceReqIds != null)
			{
				HisSereServView9FilterQuery filter = new HisSereServView9FilterQuery();
				filter.SERVICE_REQ_IDs = serviceReqIds;
				filter.IS_SPECIMEN = isSpecimen;
				return new HisSereServGet().GetView9(filter);
			}
			return null;
		}
		
		internal V_HIS_SERE_SERV_9 GetView9ById(long id)
		{
			try
			{
				return GetView9ById(id, new HisSereServView9FilterQuery());
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				param.HasException = true;
				return null;
			}
		}

		internal V_HIS_SERE_SERV_9 GetView9ById(long id, HisSereServView9FilterQuery filter)
		{
			try
			{
				return DAOWorker.HisSereServDAO.GetView9ById(id, filter.Query());
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
