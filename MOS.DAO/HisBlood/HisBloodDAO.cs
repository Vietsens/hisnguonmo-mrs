using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBlood
{
    public partial class HisBloodDAO : EntityBase
    {
        private HisBloodGet GetWorker
        {
            get
            {
                return (HisBloodGet)Worker.Get<HisBloodGet>();
            }
        }
        public List<HIS_BLOOD> Get(HisBloodSO search, CommonParam param)
        {
            List<HIS_BLOOD> result = new List<HIS_BLOOD>();
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

        public HIS_BLOOD GetById(long id, HisBloodSO search)
        {
            HIS_BLOOD result = null;
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
