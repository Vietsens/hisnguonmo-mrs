using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTreatmentBedRoom
{
    public partial class HisTreatmentBedRoomDAO : EntityBase
    {
        public List<V_HIS_TREATMENT_BED_ROOM> GetView(HisTreatmentBedRoomSO search, CommonParam param)
        {
            List<V_HIS_TREATMENT_BED_ROOM> result = new List<V_HIS_TREATMENT_BED_ROOM>();
            try
            {
                result = GetWorker.GetView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public V_HIS_TREATMENT_BED_ROOM GetViewById(long id, HisTreatmentBedRoomSO search)
        {
            V_HIS_TREATMENT_BED_ROOM result = null;

            try
            {
                result = GetWorker.GetViewById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
