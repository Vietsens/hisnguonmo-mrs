using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBed
{
    public partial class HisBedDAO : EntityBase
    {
        private HisBedGet GetWorker
        {
            get
            {
                return (HisBedGet)Worker.Get<HisBedGet>();
            }
        }

        public List<HIS_BED> Get(HisBedSO search, CommonParam param)
        {
            List<HIS_BED> result = new List<HIS_BED>();
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

        public HIS_BED GetById(long id, HisBedSO search)
        {
            HIS_BED result = null;
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
