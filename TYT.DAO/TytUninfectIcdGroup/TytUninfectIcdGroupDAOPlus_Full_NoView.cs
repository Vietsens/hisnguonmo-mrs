using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytUninfectIcdGroup
{
    public partial class TytUninfectIcdGroupDAO : EntityBase
    {
        public TYT_UNINFECT_ICD_GROUP GetByCode(string code, TytUninfectIcdGroupSO search)
        {
            TYT_UNINFECT_ICD_GROUP result = null;

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

        public Dictionary<string, TYT_UNINFECT_ICD_GROUP> GetDicByCode(TytUninfectIcdGroupSO search, CommonParam param)
        {
            Dictionary<string, TYT_UNINFECT_ICD_GROUP> result = new Dictionary<string, TYT_UNINFECT_ICD_GROUP>();
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
