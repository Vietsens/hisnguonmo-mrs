using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisVitaminA
{
    public partial class HisVitaminADAO : EntityBase
    {
        private HisVitaminAGet GetWorker
        {
            get
            {
                return (HisVitaminAGet)Worker.Get<HisVitaminAGet>();
            }
        }

        public List<HIS_VITAMIN_A> Get(HisVitaminASO search, CommonParam param)
        {
            List<HIS_VITAMIN_A> result = new List<HIS_VITAMIN_A>();
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

        public HIS_VITAMIN_A GetById(long id, HisVitaminASO search)
        {
            HIS_VITAMIN_A result = null;
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
