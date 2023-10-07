using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCashierRoom
{
    public partial class HisCashierRoomManager : BusinessBase
    {
        
        public List<V_HIS_CASHIER_ROOM> GetView(HisCashierRoomViewFilterQuery filter)
        {
            List<V_HIS_CASHIER_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_CASHIER_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisCashierRoomGet(param).GetView(filter);
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

        
        public V_HIS_CASHIER_ROOM GetViewByCode(string data)
        {
            V_HIS_CASHIER_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_CASHIER_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisCashierRoomGet(param).GetViewByCode(data);
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

        
        public V_HIS_CASHIER_ROOM GetViewByCode(string data, HisCashierRoomViewFilterQuery filter)
        {
            V_HIS_CASHIER_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_CASHIER_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisCashierRoomGet(param).GetViewByCode(data, filter);
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

        
        public V_HIS_CASHIER_ROOM GetViewById(long data)
        {
            V_HIS_CASHIER_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_CASHIER_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisCashierRoomGet(param).GetViewById(data);
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

        
        public V_HIS_CASHIER_ROOM GetViewById(long data, HisCashierRoomViewFilterQuery filter)
        {
            V_HIS_CASHIER_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_CASHIER_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisCashierRoomGet(param).GetViewById(data, filter);
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
