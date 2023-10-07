using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisRegisterGate
{
    public partial class HisRegisterGateDAO : EntityBase
    {
        public HIS_REGISTER_GATE GetByCode(string code, HisRegisterGateSO search)
        {
            HIS_REGISTER_GATE result = null;

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

        public Dictionary<string, HIS_REGISTER_GATE> GetDicByCode(HisRegisterGateSO search, CommonParam param)
        {
            Dictionary<string, HIS_REGISTER_GATE> result = new Dictionary<string, HIS_REGISTER_GATE>();
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
