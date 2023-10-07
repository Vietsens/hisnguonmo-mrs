using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisDepositReq
{
    public partial class HisDepositReqDAO : EntityBase
    {
        public List<V_HIS_DEPOSIT_REQ> GetView(HisDepositReqSO search, CommonParam param)
        {
            List<V_HIS_DEPOSIT_REQ> result = new List<V_HIS_DEPOSIT_REQ>();

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

        public HIS_DEPOSIT_REQ GetByCode(string code, HisDepositReqSO search)
        {
            HIS_DEPOSIT_REQ result = null;

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
        
        public V_HIS_DEPOSIT_REQ GetViewById(long id, HisDepositReqSO search)
        {
            V_HIS_DEPOSIT_REQ result = null;

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

        public V_HIS_DEPOSIT_REQ GetViewByCode(string code, HisDepositReqSO search)
        {
            V_HIS_DEPOSIT_REQ result = null;

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

        public Dictionary<string, HIS_DEPOSIT_REQ> GetDicByCode(HisDepositReqSO search, CommonParam param)
        {
            Dictionary<string, HIS_DEPOSIT_REQ> result = new Dictionary<string, HIS_DEPOSIT_REQ>();
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
