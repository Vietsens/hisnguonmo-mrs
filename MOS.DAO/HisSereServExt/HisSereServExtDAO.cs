using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSereServExt
{
    public partial class HisSereServExtDAO : EntityBase
    {
        private HisSereServExtGet GetWorker
        {
            get
            {
                return (HisSereServExtGet)Worker.Get<HisSereServExtGet>();
            }
        }
        public List<HIS_SERE_SERV_EXT> Get(HisSereServExtSO search, CommonParam param)
        {
            List<HIS_SERE_SERV_EXT> result = new List<HIS_SERE_SERV_EXT>();
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

        public HIS_SERE_SERV_EXT GetById(long id, HisSereServExtSO search)
        {
            HIS_SERE_SERV_EXT result = null;
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
