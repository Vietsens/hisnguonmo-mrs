using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTreatmentBedRoom
{
    public partial class HisTreatmentBedRoomManager : BusinessBase
    {
        public HisTreatmentBedRoomManager()
            : base()
        {

        }

        public HisTreatmentBedRoomManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_TREATMENT_BED_ROOM> Get(HisTreatmentBedRoomFilterQuery filter)
        {
             List<HIS_TREATMENT_BED_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_TREATMENT_BED_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentBedRoomGet(param).Get(filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public  HIS_TREATMENT_BED_ROOM GetById(long data)
        {
             HIS_TREATMENT_BED_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_TREATMENT_BED_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentBedRoomGet(param).GetById(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public  HIS_TREATMENT_BED_ROOM GetById(long data, HisTreatmentBedRoomFilterQuery filter)
        {
             HIS_TREATMENT_BED_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_TREATMENT_BED_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentBedRoomGet(param).GetById(data, filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }
    }
}
