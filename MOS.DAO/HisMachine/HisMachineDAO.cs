using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMachine
{
    public partial class HisMachineDAO : EntityBase
    {
        private HisMachineGet GetWorker
        {
            get
            {
                return (HisMachineGet)Worker.Get<HisMachineGet>();
            }
        }

        public List<HIS_MACHINE> Get(HisMachineSO search, CommonParam param)
        {
            List<HIS_MACHINE> result = new List<HIS_MACHINE>();
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

        public HIS_MACHINE GetById(long id, HisMachineSO search)
        {
            HIS_MACHINE result = null;
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
