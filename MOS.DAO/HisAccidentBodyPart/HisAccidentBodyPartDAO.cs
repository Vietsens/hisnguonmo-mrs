using Inventec.Common.Repository;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccidentBodyPart
{
    public partial class HisAccidentBodyPartDAO : EntityBase
    {
        private HisAccidentBodyPartGet GetWorker
        {
            get
            {
                return (HisAccidentBodyPartGet)Worker.Get<HisAccidentBodyPartGet>();
            }
        }
        public List<HIS_ACCIDENT_BODY_PART> Get(HisAccidentBodyPartSO search, CommonParam param)
        {
            List<HIS_ACCIDENT_BODY_PART> result = new List<HIS_ACCIDENT_BODY_PART>();
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

        public HIS_ACCIDENT_BODY_PART GetById(long id, HisAccidentBodyPartSO search)
        {
            HIS_ACCIDENT_BODY_PART result = null;
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
