using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServiceReqType
{
    public partial class HisServiceReqTypeDAO : EntityBase
    {
        public HIS_SERVICE_REQ_TYPE GetByCode(string code, HisServiceReqTypeSO search)
        {
            HIS_SERVICE_REQ_TYPE result = null;

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

        public Dictionary<string, HIS_SERVICE_REQ_TYPE> GetDicByCode(HisServiceReqTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_SERVICE_REQ_TYPE> result = new Dictionary<string, HIS_SERVICE_REQ_TYPE>();
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
