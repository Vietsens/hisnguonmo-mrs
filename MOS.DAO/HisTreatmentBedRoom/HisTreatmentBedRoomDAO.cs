using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTreatmentBedRoom
{
    public partial class HisTreatmentBedRoomDAO : EntityBase
    {
        private HisTreatmentBedRoomGet GetWorker
        {
            get
            {
                return (HisTreatmentBedRoomGet)Worker.Get<HisTreatmentBedRoomGet>();
            }
        }
        public List<HIS_TREATMENT_BED_ROOM> Get(HisTreatmentBedRoomSO search, CommonParam param)
        {
            List<HIS_TREATMENT_BED_ROOM> result = new List<HIS_TREATMENT_BED_ROOM>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_TREATMENT_BED_ROOM GetById(long id, HisTreatmentBedRoomSO search)
        {
            HIS_TREATMENT_BED_ROOM result = null;
            try
            {
                result = GetWorker.GetById(id, search);
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
