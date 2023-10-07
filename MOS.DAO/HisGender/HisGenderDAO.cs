using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisGender
{
    public partial class HisGenderDAO : EntityBase
    {
        private HisGenderGet GetWorker
        {
            get
            {
                return (HisGenderGet)Worker.Get<HisGenderGet>();
            }
        }
        public List<HIS_GENDER> Get(HisGenderSO search, CommonParam param)
        {
            List<HIS_GENDER> result = new List<HIS_GENDER>();
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

        public HIS_GENDER GetById(long id, HisGenderSO search)
        {
            HIS_GENDER result = null;
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
