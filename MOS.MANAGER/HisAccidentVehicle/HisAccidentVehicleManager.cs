using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentVehicle
{
    public partial class HisAccidentVehicleManager : BusinessBase
    {
        public HisAccidentVehicleManager()
            : base()
        {

        }

        public HisAccidentVehicleManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_ACCIDENT_VEHICLE> Get(HisAccidentVehicleFilterQuery filter)
        {
             List<HIS_ACCIDENT_VEHICLE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_ACCIDENT_VEHICLE> resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentVehicleGet(param).Get(filter);
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

        
        public  HIS_ACCIDENT_VEHICLE GetById(long data)
        {
             HIS_ACCIDENT_VEHICLE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ACCIDENT_VEHICLE resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentVehicleGet(param).GetById(data);
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

        
        public  HIS_ACCIDENT_VEHICLE GetById(long data, HisAccidentVehicleFilterQuery filter)
        {
             HIS_ACCIDENT_VEHICLE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ACCIDENT_VEHICLE resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentVehicleGet(param).GetById(data, filter);
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

        
        public  HIS_ACCIDENT_VEHICLE GetByCode(string data)
        {
             HIS_ACCIDENT_VEHICLE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ACCIDENT_VEHICLE resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentVehicleGet(param).GetByCode(data);
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

        
        public  HIS_ACCIDENT_VEHICLE GetByCode(string data, HisAccidentVehicleFilterQuery filter)
        {
             HIS_ACCIDENT_VEHICLE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ACCIDENT_VEHICLE resultData = null;
                if (valid)
                {
                    resultData = new HisAccidentVehicleGet(param).GetByCode(data, filter);
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
