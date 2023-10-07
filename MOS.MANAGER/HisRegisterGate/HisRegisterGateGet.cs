using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRegisterGate
{
    partial class HisRegisterGateGet : BusinessBase
    {
        internal HisRegisterGateGet()
            : base()
        {

        }

        internal HisRegisterGateGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_REGISTER_GATE> Get(HisRegisterGateFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRegisterGateDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REGISTER_GATE GetById(long id)
        {
            try
            {
                return GetById(id, new HisRegisterGateFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REGISTER_GATE GetById(long id, HisRegisterGateFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRegisterGateDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
