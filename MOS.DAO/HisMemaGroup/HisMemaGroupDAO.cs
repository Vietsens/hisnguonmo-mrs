using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMemaGroup
{
    public partial class HisMemaGroupDAO : EntityBase
    {
        private HisMemaGroupGet GetWorker
        {
            get
            {
                return (HisMemaGroupGet)Worker.Get<HisMemaGroupGet>();
            }
        }
        public List<HIS_MEMA_GROUP> Get(HisMemaGroupSO search, CommonParam param)
        {
            List<HIS_MEMA_GROUP> result = new List<HIS_MEMA_GROUP>();
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

        public HIS_MEMA_GROUP GetById(long id, HisMemaGroupSO search)
        {
            HIS_MEMA_GROUP result = null;
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
