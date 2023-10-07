using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisDiseaseRelation
{
    public partial class HisDiseaseRelationDAO : EntityBase
    {
        private HisDiseaseRelationGet GetWorker
        {
            get
            {
                return (HisDiseaseRelationGet)Worker.Get<HisDiseaseRelationGet>();
            }
        }
        public List<HIS_DISEASE_RELATION> Get(HisDiseaseRelationSO search, CommonParam param)
        {
            List<HIS_DISEASE_RELATION> result = new List<HIS_DISEASE_RELATION>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_DISEASE_RELATION GetById(long id, HisDiseaseRelationSO search)
        {
            HIS_DISEASE_RELATION result = null;
            try
            {
                result = GetWorker.GetById(id, search);
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
