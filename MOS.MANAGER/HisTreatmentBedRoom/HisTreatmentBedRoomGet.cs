using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTreatmentBedRoom
{
    partial class HisTreatmentBedRoomGet : BusinessBase
    {
        internal HisTreatmentBedRoomGet()
            : base()
        {

        }

        internal HisTreatmentBedRoomGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_TREATMENT_BED_ROOM> Get(HisTreatmentBedRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentBedRoomDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_TREATMENT_BED_ROOM> GetView(HisTreatmentBedRoomViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentBedRoomDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_TREATMENT_BED_ROOM> GetViewCurrentIn(long? bedRoomId, long? treatmentId)
        {
            try
            {
                HisTreatmentBedRoomViewFilterQuery filter = new HisTreatmentBedRoomViewFilterQuery();
                filter.BED_ROOM_ID = bedRoomId;
                filter.TREATMENT_ID = treatmentId;
                filter.IS_IN_ROOM = true;
                return this.GetView(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_TREATMENT_BED_ROOM> GetViewCurrentInByTreatmentId(long treatmentId)
        {
            return this.GetViewCurrentIn(null, treatmentId);
        }

        internal List<V_HIS_TREATMENT_BED_ROOM> GetViewCurrentInByBedRoomId(long bedRoomId)
        {
            return this.GetViewCurrentIn(bedRoomId, null);
        }

        internal List<HIS_TREATMENT_BED_ROOM> GetCurrentInByBedRoomId(long bedRoomId)
        {
            return this.GetCurrentIn(bedRoomId, null);
        }

        internal List<HIS_TREATMENT_BED_ROOM> GetCurrentInByTreatmentId(long treatmentId)
        {
            return this.GetCurrentIn(null, treatmentId);
        }

        internal List<HIS_TREATMENT_BED_ROOM> GetCurrentIn(long? bedRoomId, long? treatmentId)
        {
            try
            {
                HisTreatmentBedRoomFilterQuery filter = new HisTreatmentBedRoomFilterQuery();
                filter.BED_ROOM_ID = bedRoomId;
                filter.TREATMENT_ID = treatmentId;
                filter.IS_IN_ROOM = true;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TREATMENT_BED_ROOM GetById(long id)
        {
            try
            {
                return GetById(id, new HisTreatmentBedRoomFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TREATMENT_BED_ROOM GetById(long id, HisTreatmentBedRoomFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTreatmentBedRoomDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_TREATMENT_BED_ROOM> GetByTreatmentId(long id)
        {
            try
            {
                HisTreatmentBedRoomFilterQuery filter = new HisTreatmentBedRoomFilterQuery();
                filter.TREATMENT_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_TREATMENT_BED_ROOM> GetByBedRoomId(long id)
        {
            try
            {
                HisTreatmentBedRoomFilterQuery filter = new HisTreatmentBedRoomFilterQuery();
                filter.BED_ROOM_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_TREATMENT_BED_ROOM> GetByBedId(long id)
        {
            try
            {
                HisTreatmentBedRoomFilterQuery filter = new HisTreatmentBedRoomFilterQuery();
                filter.BED_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TREATMENT_BED_ROOM GetViewById(long id)
        {
            HisTreatmentBedRoomViewFilterQuery filter = new HisTreatmentBedRoomViewFilterQuery();
            filter.ID = id;
            List<V_HIS_TREATMENT_BED_ROOM> data = this.GetView(filter);
            return data != null ? data[0] : null;
        }
    }
}
