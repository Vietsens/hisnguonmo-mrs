using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisCashierRoom
{
    public partial class HisCashierRoomDAO : EntityBase
    {
        public List<V_HIS_CASHIER_ROOM> GetView(HisCashierRoomSO search, CommonParam param)
        {
            List<V_HIS_CASHIER_ROOM> result = new List<V_HIS_CASHIER_ROOM>();

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

        public HIS_CASHIER_ROOM GetByCode(string code, HisCashierRoomSO search)
        {
            HIS_CASHIER_ROOM result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
        
        public V_HIS_CASHIER_ROOM GetViewById(long id, HisCashierRoomSO search)
        {
            V_HIS_CASHIER_ROOM result = null;

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

        public V_HIS_CASHIER_ROOM GetViewByCode(string code, HisCashierRoomSO search)
        {
            V_HIS_CASHIER_ROOM result = null;

            try
            {
                result = GetWorker.GetViewByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public Dictionary<string, HIS_CASHIER_ROOM> GetDicByCode(HisCashierRoomSO search, CommonParam param)
        {
            Dictionary<string, HIS_CASHIER_ROOM> result = new Dictionary<string, HIS_CASHIER_ROOM>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }
    }
}
