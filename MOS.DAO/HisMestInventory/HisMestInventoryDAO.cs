using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMestInventory
{
    public partial class HisMestInventoryDAO : EntityBase
    {
        private HisMestInventoryGet GetWorker
        {
            get
            {
                return (HisMestInventoryGet)Worker.Get<HisMestInventoryGet>();
            }
        }
        public List<HIS_MEST_INVENTORY> Get(HisMestInventorySO search, CommonParam param)
        {
            List<HIS_MEST_INVENTORY> result = new List<HIS_MEST_INVENTORY>();
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

        public HIS_MEST_INVENTORY GetById(long id, HisMestInventorySO search)
        {
            HIS_MEST_INVENTORY result = null;
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
