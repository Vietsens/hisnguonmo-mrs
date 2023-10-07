using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServ
{
	partial class HisSereServGet : GetBase
	{
		internal List<V_HIS_SERE_SERV_11> GetView11(HisSereServView11FilterQuery filter)
		{
			try
			{
				return DAOWorker.HisSereServDAO.GetView11(filter.Query(), param);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				param.HasException = true;
				return null;
			}
		}

		internal List<V_HIS_SERE_SERV_11> GetView11ByIds(List<long> ids)
		{
			try
			{
				if (ids != null)
				{
					HisSereServView11FilterQuery filter = new HisSereServView11FilterQuery();
					filter.IDs = ids;
					return this.GetView11(filter);
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

		internal List<V_HIS_SERE_SERV_11> GetView11ByServiceReqId(long serviceReqId)
		{
			HisSereServView11FilterQuery filter = new HisSereServView11FilterQuery();
			filter.SERVICE_REQ_ID = serviceReqId;
			return new HisSereServGet().GetView11(filter);
		}

		internal List<V_HIS_SERE_SERV_11> GetView11ByServiceReqIdAndIsSpecimen(long serviceReqId, bool isSpecimen)
		{
			HisSereServView11FilterQuery filter = new HisSereServView11FilterQuery();
			filter.SERVICE_REQ_ID = serviceReqId;
			filter.IS_SPECIMEN = isSpecimen;
			return new HisSereServGet().GetView11(filter);
		}

		internal List<V_HIS_SERE_SERV_11> GetView11ByServiceReqIdsAndIsSpecimen(List<long> serviceReqIds, bool isSpecimen)
		{
			if (serviceReqIds != null)
			{
				HisSereServView11FilterQuery filter = new HisSereServView11FilterQuery();
				filter.SERVICE_REQ_IDs = serviceReqIds;
				filter.IS_SPECIMEN = isSpecimen;
				return new HisSereServGet().GetView11(filter);
			}
			return null;
		}
		
		internal V_HIS_SERE_SERV_11 GetView11ById(long id)
		{
			try
			{
				return GetView11ById(id, new HisSereServView11FilterQuery());
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				param.HasException = true;
				return null;
			}
		}

		internal V_HIS_SERE_SERV_11 GetView11ById(long id, HisSereServView11FilterQuery filter)
		{
			try
			{
				return DAOWorker.HisSereServDAO.GetView11ById(id, filter.Query());
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
