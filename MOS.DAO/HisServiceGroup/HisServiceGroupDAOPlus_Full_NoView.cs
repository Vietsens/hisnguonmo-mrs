using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServiceGroup
{
    public partial class HisServiceGroupDAO : EntityBase
    {
        public HIS_SERVICE_GROUP GetByCode(string code, HisServiceGroupSO search)
        {
            HIS_SERVICE_GROUP result = null;

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

        public Dictionary<string, HIS_SERVICE_GROUP> GetDicByCode(HisServiceGroupSO search, CommonParam param)
        {
            Dictionary<string, HIS_SERVICE_GROUP> result = new Dictionary<string, HIS_SERVICE_GROUP>();
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
