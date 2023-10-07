using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisDiseaseRelation
{
    public partial class HisDiseaseRelationDAO : EntityBase
    {
        public HIS_DISEASE_RELATION GetByCode(string code, HisDiseaseRelationSO search)
        {
            HIS_DISEASE_RELATION result = null;

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

        public Dictionary<string, HIS_DISEASE_RELATION> GetDicByCode(HisDiseaseRelationSO search, CommonParam param)
        {
            Dictionary<string, HIS_DISEASE_RELATION> result = new Dictionary<string, HIS_DISEASE_RELATION>();
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
