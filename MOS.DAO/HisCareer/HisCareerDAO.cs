using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisCareer
{
    public partial class HisCareerDAO : EntityBase
    {
        private HisCareerGet GetWorker
        {
            get
            {
                return (HisCareerGet)Worker.Get<HisCareerGet>();
            }
        }
        public List<HIS_CAREER> Get(HisCareerSO search, CommonParam param)
        {
            List<HIS_CAREER> result = new List<HIS_CAREER>();
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

        public HIS_CAREER GetById(long id, HisCareerSO search)
        {
            HIS_CAREER result = null;
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
