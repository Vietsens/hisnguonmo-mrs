using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisRegisterGate
{
    public partial class HisRegisterGateDAO : EntityBase
    {
        private HisRegisterGateGet GetWorker
        {
            get
            {
                return (HisRegisterGateGet)Worker.Get<HisRegisterGateGet>();
            }
        }
        public List<HIS_REGISTER_GATE> Get(HisRegisterGateSO search, CommonParam param)
        {
            List<HIS_REGISTER_GATE> result = new List<HIS_REGISTER_GATE>();
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

        public HIS_REGISTER_GATE GetById(long id, HisRegisterGateSO search)
        {
            HIS_REGISTER_GATE result = null;
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
