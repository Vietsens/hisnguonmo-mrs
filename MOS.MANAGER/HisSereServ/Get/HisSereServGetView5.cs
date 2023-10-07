using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServ
{
	partial class HisSereServGet : GetBase
	{
		internal List<V_HIS_SERE_SERV_5> GetView5(HisSereServView5FilterQuery filter)
		{
			try
			{
				return DAOWorker.HisSereServDAO.GetView5(filter.Query(), param);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				param.HasException = true;
				return null;
			}
		}

		internal List<V_HIS_SERE_SERV_5> GetView5ByIds(List<long> ids)
		{
			try
			{
				if (ids != null)
				{
					HisSereServView5FilterQuery filter = new HisSereServView5FilterQuery();
					filter.IDs = ids;
					return this.GetView5(filter);
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

		internal List<V_HIS_SERE_SERV_5> GetView5ByServiceReqId(long serviceReqId)
		{
			HisSereServView5FilterQuery filter = new HisSereServView5FilterQuery();
			filter.SERVICE_REQ_ID = serviceReqId;
			return new HisSereServGet().GetView5(filter);
		}

		internal List<V_HIS_SERE_SERV_5> GetView5ByServiceReqIdAndIsSpecimen(long serviceReqId, bool isSpecimen)
		{
			HisSereServView5FilterQuery filter = new HisSereServView5FilterQuery();
			filter.SERVICE_REQ_ID = serviceReqId;
			filter.IS_SPECIMEN = isSpecimen;
			return new HisSereServGet().GetView5(filter);
		}

		internal List<V_HIS_SERE_SERV_5> GetView5ByServiceReqIdsAndIsSpecimen(List<long> serviceReqIds, bool isSpecimen)
		{
			if (serviceReqIds != null)
			{
				HisSereServView5FilterQuery filter = new HisSereServView5FilterQuery();
				filter.SERVICE_REQ_IDs = serviceReqIds;
				filter.IS_SPECIMEN = isSpecimen;
				return new HisSereServGet().GetView5(filter);
			}
			return null;
		}
		
		internal V_HIS_SERE_SERV_5 GetView5ById(long id)
		{
			try
			{
				return GetView5ById(id, new HisSereServView5FilterQuery());
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				param.HasException = true;
				return null;
			}
		}

		internal V_HIS_SERE_SERV_5 GetView5ById(long id, HisSereServView5FilterQuery filter)
		{
			try
			{
				return DAOWorker.HisSereServDAO.GetView5ById(id, filter.Query());
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
