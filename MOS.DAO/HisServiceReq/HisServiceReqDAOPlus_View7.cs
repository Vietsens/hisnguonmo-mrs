using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServiceReq
{
    public partial class HisServiceReqDAO : EntityBase
    {
        public List<V_HIS_SERVICE_REQ_7> GetView7(HisServiceReqSO search, CommonParam param)
        {
            List<V_HIS_SERVICE_REQ_7> result = new List<V_HIS_SERVICE_REQ_7>();

            try
            {
                result = GetWorker.GetView7(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }
        
        public V_HIS_SERVICE_REQ_7 GetView7ById(long id, HisServiceReqSO search)
        {
            V_HIS_SERVICE_REQ_7 result = null;

            try
            {
                result = GetWorker.GetView7ById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public V_HIS_SERVICE_REQ_7 GetView7ByCode(string code, HisServiceReqSO search)
        {
            V_HIS_SERVICE_REQ_7 result = null;

            try
            {
                result = GetWorker.GetView7ByCode(code, search);
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
