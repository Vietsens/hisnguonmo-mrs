using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExecuteGroup
{
    public partial class HisExecuteGroupDAO : EntityBase
    {
        public HIS_EXECUTE_GROUP GetByCode(string code, HisExecuteGroupSO search)
        {
            HIS_EXECUTE_GROUP result = null;

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

        public Dictionary<string, HIS_EXECUTE_GROUP> GetDicByCode(HisExecuteGroupSO search, CommonParam param)
        {
            Dictionary<string, HIS_EXECUTE_GROUP> result = new Dictionary<string, HIS_EXECUTE_GROUP>();
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
