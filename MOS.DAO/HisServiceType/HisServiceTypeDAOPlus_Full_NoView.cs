using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServiceType
{
    public partial class HisServiceTypeDAO : EntityBase
    {
        public HIS_SERVICE_TYPE GetByCode(string code, HisServiceTypeSO search)
        {
            HIS_SERVICE_TYPE result = null;

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

        public Dictionary<string, HIS_SERVICE_TYPE> GetDicByCode(HisServiceTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_SERVICE_TYPE> result = new Dictionary<string, HIS_SERVICE_TYPE>();
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
