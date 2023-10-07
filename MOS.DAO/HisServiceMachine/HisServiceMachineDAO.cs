using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServiceMachine
{
    public partial class HisServiceMachineDAO : EntityBase
    {
        private HisServiceMachineGet GetWorker
        {
            get
            {
                return (HisServiceMachineGet)Worker.Get<HisServiceMachineGet>();
            }
        }

        public List<HIS_SERVICE_MACHINE> Get(HisServiceMachineSO search, CommonParam param)
        {
            List<HIS_SERVICE_MACHINE> result = new List<HIS_SERVICE_MACHINE>();
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

        public HIS_SERVICE_MACHINE GetById(long id, HisServiceMachineSO search)
        {
            HIS_SERVICE_MACHINE result = null;
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
