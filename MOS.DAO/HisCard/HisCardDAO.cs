using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisCard
{
    public partial class HisCardDAO : EntityBase
    {
        private HisCardGet GetWorker
        {
            get
            {
                return (HisCardGet)Worker.Get<HisCardGet>();
            }
        }
        public List<HIS_CARD> Get(HisCardSO search, CommonParam param)
        {
            List<HIS_CARD> result = new List<HIS_CARD>();
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

        public HIS_CARD GetById(long id, HisCardSO search)
        {
            HIS_CARD result = null;
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
