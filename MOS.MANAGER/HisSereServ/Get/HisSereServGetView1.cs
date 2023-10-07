using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServ
{
	partial class HisSereServGet : GetBase
	{
		internal List<V_HIS_SERE_SERV_1> GetView1(HisSereServView1FilterQuery filter)
		{
			try
			{
				return DAOWorker.HisSereServDAO.GetView1(filter.Query(), param);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				param.HasException = true;
				return null;
			}
		}

        internal List<V_HIS_SERE_SERV_1> GetView1ByIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisSereServView1FilterQuery filter = new HisSereServView1FilterQuery();
                    filter.IDs = ids;
                    return this.GetView1(filter);
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

		internal List<V_HIS_SERE_SERV_1> GetView1ByServiceReqId(long serviceReqId)
		{
			HisSereServView1FilterQuery filter = new HisSereServView1FilterQuery();
			filter.SERVICE_REQ_ID = serviceReqId;
			return new HisSereServGet().GetView1(filter);
		}

        internal List<V_HIS_SERE_SERV_1> GetView1ByServiceReqIdAndIsSpecimen(long serviceReqId, bool isSpecimen)
        {
            HisSereServView1FilterQuery filter = new HisSereServView1FilterQuery();
            filter.SERVICE_REQ_ID = serviceReqId;
            filter.IS_SPECIMEN = isSpecimen;
            return new HisSereServGet().GetView1(filter);
        }

        internal List<V_HIS_SERE_SERV_1> GetView1ByServiceReqIdsAndIsSpecimen(List<long> serviceReqIds, bool isSpecimen)
        {
            if (serviceReqIds != null)
            {
                HisSereServView1FilterQuery filter = new HisSereServView1FilterQuery();
                filter.SERVICE_REQ_IDs = serviceReqIds;
                filter.IS_SPECIMEN = isSpecimen;
                return new HisSereServGet().GetView1(filter);
            }
            return null;
        }

        internal List<V_HIS_SERE_SERV_1> GetView1ByTreatmentId(long treatmentId)
        {
            HisSereServView1FilterQuery filter = new HisSereServView1FilterQuery();
            filter.TREATMENT_ID = treatmentId;
            return new HisSereServGet().GetView1(filter);
        }
		
        internal V_HIS_SERE_SERV_1 GetView1ById(long id)
        {
            try
            {
                return GetView1ById(id, new HisSereServView1FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERE_SERV_1 GetView1ById(long id, HisSereServView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServDAO.GetView1ById(id, filter.Query());
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
