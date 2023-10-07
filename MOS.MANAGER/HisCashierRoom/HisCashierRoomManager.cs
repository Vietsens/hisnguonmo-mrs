using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCashierRoom
{
    public partial class HisCashierRoomManager : BusinessBase
    {
        public HisCashierRoomManager()
            : base()
        {

        }

        public HisCashierRoomManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_CASHIER_ROOM> Get(HisCashierRoomFilterQuery filter)
        {
             List<HIS_CASHIER_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_CASHIER_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisCashierRoomGet(param).Get(filter);
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

        
        public  HIS_CASHIER_ROOM GetById(long data)
        {
             HIS_CASHIER_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_CASHIER_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisCashierRoomGet(param).GetById(data);
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

        
        public  HIS_CASHIER_ROOM GetById(long data, HisCashierRoomFilterQuery filter)
        {
             HIS_CASHIER_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_CASHIER_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisCashierRoomGet(param).GetById(data, filter);
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

        
        public  HIS_CASHIER_ROOM GetByCode(string data)
        {
             HIS_CASHIER_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_CASHIER_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisCashierRoomGet(param).GetByCode(data);
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

        
        public  HIS_CASHIER_ROOM GetByCode(string data, HisCashierRoomFilterQuery filter)
        {
             HIS_CASHIER_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_CASHIER_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisCashierRoomGet(param).GetByCode(data, filter);
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
