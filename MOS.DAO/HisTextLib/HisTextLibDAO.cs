using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTextLib
{
    public partial class HisTextLibDAO : EntityBase
    {
        private HisTextLibGet GetWorker
        {
            get
            {
                return (HisTextLibGet)Worker.Get<HisTextLibGet>();
            }
        }
        public List<HIS_TEXT_LIB> Get(HisTextLibSO search, CommonParam param)
        {
            List<HIS_TEXT_LIB> result = new List<HIS_TEXT_LIB>();
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

        public HIS_TEXT_LIB GetById(long id, HisTextLibSO search)
        {
            HIS_TEXT_LIB result = null;
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
