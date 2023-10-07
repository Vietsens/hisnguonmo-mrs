using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytUninfectIcd
{
    public partial class TytUninfectIcdDAO : EntityBase
    {
        public TYT_UNINFECT_ICD GetByCode(string code, TytUninfectIcdSO search)
        {
            TYT_UNINFECT_ICD result = null;

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

        public Dictionary<string, TYT_UNINFECT_ICD> GetDicByCode(TytUninfectIcdSO search, CommonParam param)
        {
            Dictionary<string, TYT_UNINFECT_ICD> result = new Dictionary<string, TYT_UNINFECT_ICD>();
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
