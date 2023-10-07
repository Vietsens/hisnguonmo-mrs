using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServ
{
	partial class HisSereServGet : GetBase
	{
		internal List<V_HIS_SERE_SERV_7> GetView7(HisSereServView7FilterQuery filter)
		{
			try
			{
				return DAOWorker.HisSereServDAO.GetView7(filter.Query(), param);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				param.HasException = true;
				return null;
			}
		}

		internal List<V_HIS_SERE_SERV_7> GetView7ByIds(List<long> ids)
		{
			try
			{
				if (ids != null)
				{
					HisSereServView7FilterQuery filter = new HisSereServView7FilterQuery();
					filter.IDs = ids;
					return this.GetView7(filter);
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

		internal List<V_HIS_SERE_SERV_7> GetView7ByServiceReqId(long serviceReqId)
		{
			HisSereServView7FilterQuery filter = new HisSereServView7FilterQuery();
			filter.SERVICE_REQ_ID = serviceReqId;
			return new HisSereServGet().GetView7(filter);
		}

		internal List<V_HIS_SERE_SERV_7> GetView7ByServiceReqIdAndIsSpecimen(long serviceReqId, bool isSpecimen)
		{
			HisSereServView7FilterQuery filter = new HisSereServView7FilterQuery();
			filter.SERVICE_REQ_ID = serviceReqId;
			filter.IS_SPECIMEN = isSpecimen;
			return new HisSereServGet().GetView7(filter);
		}

		internal List<V_HIS_SERE_SERV_7> GetView7ByServiceReqIdsAndIsSpecimen(List<long> serviceReqIds, bool isSpecimen)
		{
			if (serviceReqIds != null)
			{
				HisSereServView7FilterQuery filter = new HisSereServView7FilterQuery();
				filter.SERVICE_REQ_IDs = serviceReqIds;
				filter.IS_SPECIMEN = isSpecimen;
				return new HisSereServGet().GetView7(filter);
			}
			return null;
		}
		
		internal V_HIS_SERE_SERV_7 GetView7ById(long id)
		{
			try
			{
				return GetView7ById(id, new HisSereServView7FilterQuery());
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				param.HasException = true;
				return null;
			}
		}

		internal V_HIS_SERE_SERV_7 GetView7ById(long id, HisSereServView7FilterQuery filter)
		{
			try
			{
				return DAOWorker.HisSereServDAO.GetView7ById(id, filter.Query());
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
