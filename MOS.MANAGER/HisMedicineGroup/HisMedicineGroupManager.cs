using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineGroup
{
    public partial class HisMedicineGroupManager : BusinessBase
    {
        public HisMedicineGroupManager()
            : base()
        {

        }

        public HisMedicineGroupManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_MEDICINE_GROUP> Get(HisMedicineGroupFilterQuery filter)
        {
             List<HIS_MEDICINE_GROUP> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEDICINE_GROUP> resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineGroupGet(param).Get(filter);
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

        
        public  HIS_MEDICINE_GROUP GetById(long data)
        {
             HIS_MEDICINE_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDICINE_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineGroupGet(param).GetById(data);
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

        
        public  HIS_MEDICINE_GROUP GetById(long data, HisMedicineGroupFilterQuery filter)
        {
             HIS_MEDICINE_GROUP result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEDICINE_GROUP resultData = null;
                if (valid)
                {
                    resultData = new HisMedicineGroupGet(param).GetById(data, filter);
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
