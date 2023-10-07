using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisCashierRoom
{
    public partial class HisCashierRoomDAO : EntityBase
    {
        private HisCashierRoomGet GetWorker
        {
            get
            {
                return (HisCashierRoomGet)Worker.Get<HisCashierRoomGet>();
            }
        }
        public List<HIS_CASHIER_ROOM> Get(HisCashierRoomSO search, CommonParam param)
        {
            List<HIS_CASHIER_ROOM> result = new List<HIS_CASHIER_ROOM>();
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

        public HIS_CASHIER_ROOM GetById(long id, HisCashierRoomSO search)
        {
            HIS_CASHIER_ROOM result = null;
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
